using PluginInterface;
using System.Reflection;
using System.Text;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using DynamicExpresso;
using MQTTnet.Server;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace Plugin
{
    public class DeviceThread : IDisposable
    {
        private readonly MqttServer _mqttServer;
        private readonly ILogger _logger;
        public readonly Device Device;
        public readonly IDriver Driver;
        private readonly string _projectId;
        private readonly MessageService _messageService;
        private Interpreter? _interpreter;
        // 缓存所有标记 MethodAttribute 的方法委托，避免每次调用反射
        public readonly Dictionary<string, Func<DriverAddressIoArgModel, DriverReturnValueModel>> MethodDelegates;
        private Task? _task;
        private readonly DateTime _tsStartDt = new(1970, 1, 1);
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(true);

        public DeviceThread(Device device, IDriver driver, string projectId, MessageService messageService,
            MqttServer mqttServer, ILogger logger)
        {
            _messageService = messageService;
            _messageService.OnExcRpc += MyMqttClient_OnExcRpc;
            Device = device;
            Driver = driver;
            _projectId = projectId;
            _interpreter = new Interpreter();
            _logger = logger;
            _mqttServer = mqttServer;
            MethodDelegates = new Dictionary<string, Func<DriverAddressIoArgModel, DriverReturnValueModel>>();

            // 将带有 MethodAttribute 的方法转换为委托，存入字典，提升调用效率
            foreach (var methodInfo in Driver.GetType().GetMethods().Where(x => x.GetCustomAttribute(typeof(MethodAttribute)) != null))
            {
                try
                {
                    var del = (Func<DriverAddressIoArgModel, DriverReturnValueModel>)Delegate.CreateDelegate(
                        typeof(Func<DriverAddressIoArgModel, DriverReturnValueModel>), Driver, methodInfo);
                    MethodDelegates[methodInfo.Name] = del;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"无法创建委托 for method {methodInfo.Name}");
                }
            }

            if (Device.AutoStart)
            {
                _logger.LogInformation($"线程已启动:{Device.DeviceName}");
                if (Device.DeviceVariables != null)
                {
                    foreach (var item in Device.DeviceVariables)
                    {
                        item.StatusType = VaribaleStatusTypeEnum.Bad;
                        if (string.IsNullOrWhiteSpace(item.Alias))
                            item.Alias = string.Empty;
                    }
                }
                _task = CreateThreadAsync(_tokenSource.Token);
            }
        }

        private async Task CreateThreadAsync(CancellationToken token)
        {
            try
            {
                await Task.Delay(5000, token);
                // 上传客户端属性
                foreach (var group in Device.DeviceVariables!.GroupBy(x => x.Alias))
                {
                    var deviceName = string.IsNullOrWhiteSpace(group.Key) ? Device.DeviceName : group.Key;
                    var configDict = Device.DeviceConfigs
                        .Where(x => x.DataSide == DataSide.ClientSide || x.DataSide == DataSide.AnySide)
                        .ToDictionary(x => x.DeviceConfigName, x => x.Value);
                    await _messageService.UploadAttributeAsync(deviceName, configDict);
                }

                while (!token.IsCancellationRequested)
                {
                    _resetEvent.Wait(token);
                    try
                    {
                        if (Driver.IsConnected)
                        {
                            foreach (var group in Device.DeviceVariables
                                     .Where(x => x.ProtectType != ProtectTypeEnum.WriteOnly)
                                     .GroupBy(x => x.Alias))
                            {
                                var deviceName = string.IsNullOrWhiteSpace(group.Key) ? Device.DeviceName : group.Key;
                                var sendModel = new Dictionary<string, List<PayLoad>>
                                {
                                    { deviceName, new List<PayLoad>() }
                                };

                                if (group.Any())
                                {
                                    var payLoadTrigger = new PayLoad() { Values = new Dictionary<string, object>() };
                                    bool canPub = false;
                                    var triggerVariables = group.Where(x => x.IsTrigger).ToList();
                                    await ReadVariablesAsync(triggerVariables, payLoadTrigger, _mqttServer, token);
                                    var triggerValues = triggerVariables.ToDictionary(x => x.Name, x => x.CookedValue);

                                    var payLoadUnTrigger = new PayLoad() { Values = new Dictionary<string, object>() };
                                    if (triggerValues.Values.Any(x => x is true) || !triggerVariables.Any())
                                    {
                                        var nonTriggerVars = group.Where(x => !triggerVariables.Select(y => y.ID).Contains(x.ID)).ToList();
                                        await ReadVariablesAsync(nonTriggerVars, payLoadUnTrigger, _mqttServer, token);
                                        canPub = true;
                                    }

                                    if (canPub)
                                    {
                                        var payLoad = new PayLoad()
                                        {
                                            Values = group
                                                .Where(x => x.StatusType == VaribaleStatusTypeEnum.Good && x.IsUpload)
                                                .ToDictionary(x => x.Name, x => x.CookedValue),
                                            DeviceStatus = payLoadTrigger.DeviceStatus
                                        };
                                        payLoad.TS = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds;
                                        payLoad.DeviceStatus = DeviceStatusTypeEnum.Good;
                                        sendModel[deviceName] = new List<PayLoad> { payLoad };
                                        await _messageService.PublishTelemetryAsync(deviceName, Device, sendModel);
                                    }

                                    if (group.Any(x => x.StatusType == VaribaleStatusTypeEnum.Bad))
                                        _messageService?.DeviceDisconnected(deviceName, Device);
                                }
                            }

                            // 若所有变量状态异常且连接仍正常，则关闭连接
                            if (Device.DeviceVariables.All(x => x.StatusType != VaribaleStatusTypeEnum.Good) && Driver.IsConnected)
                            {
                                Driver.Close();
                                Driver.Dispose();
                            }
                        }
                        else
                        {
                            foreach (var group in Device.DeviceVariables!.GroupBy(x => x.Alias))
                            {
                                var deviceName = string.IsNullOrWhiteSpace(group.Key) ? Device.DeviceName : group.Key;
                                _messageService?.DeviceDisconnected(deviceName, Device);
                            }

                            if (Driver.Connect())
                            {
                                foreach (var group in Device.DeviceVariables!.GroupBy(x => x.Alias))
                                {
                                    var deviceName = string.IsNullOrWhiteSpace(group.Key) ? Device.DeviceName : group.Key;
                                    _messageService?.DeviceConnected(deviceName, Device);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"线程循环异常,{Device.DeviceName}");
                    }
                    await Task.Delay(Device.DeviceVariables!.Any() ? (int)Driver.MinPeriod : 10000, token);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"线程取消:{Device.DeviceName}");
            }
        }

        private async Task ReadVariablesAsync(List<DeviceVariable> variables, PayLoad payLoad, MqttServer mqttServer, CancellationToken token)
        {
            if (!variables.Any())
                return;
            foreach (var item in variables.OrderBy(x => x.Index))
            {
                var ret = new DriverReturnValueModel();
                var ioarg = new DriverAddressIoArgModel
                {
                    ID = item.ID,
                    Address = item.DeviceAddress,
                    ValueType = item.DataType,
                    EndianType = item.EndianType
                };

                if (!MethodDelegates.TryGetValue(item.Method, out var methodDelegate))
                {
                    ret.StatusType = VaribaleStatusTypeEnum.MethodError;
                }
                else
                {
                    try
                    {
                        ret = methodDelegate(ioarg);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"调用方法 {item.Method} 异常");
                        ret.StatusType = VaribaleStatusTypeEnum.MethodError;
                    }
                }

                ret.Timestamp = DateTime.Now;
                item.EnqueueVariable(ret.Value);

                if (ret.StatusType == VaribaleStatusTypeEnum.Good && !string.IsNullOrWhiteSpace(item.Expressions?.Trim()))
                {
                    var expressionText = DealMysqlStr(item.Expressions)
                        .Replace("raw", item.Values[0] is bool ? $"Convert.ToBoolean(\"{item.Values[0]}\")" : item.Values[0]?.ToString())
                        .Replace("$ppv", item.Values[2] is bool ? $"Convert.ToBoolean(\"{item.Values[2]}\")" : item.Values[2]?.ToString())
                        .Replace("$pv", item.Values[1] is bool ? $"Convert.ToBoolean(\"{item.Values[1]}\")" : item.Values[1]?.ToString());

                    try
                    {
                        ret.CookedValue = _interpreter!.Eval(expressionText);
                    }
                    catch (Exception)
                    {
                        ret.Message = $"表达式错误：{expressionText}";
                        ret.StatusType = VaribaleStatusTypeEnum.ExpressionError;
                    }
                }
                else
                {
                    ret.CookedValue = ret.Value;
                }

                item.EnqueueCookedVariable(ret.CookedValue);
                payLoad.Values[item.Name] = ret.CookedValue;
                ret.VarId = item.ID;
                item.Value = ret.Value;
                item.CookedValue = ret.CookedValue;
                item.StatusType = ret.StatusType;
                item.Timestamp = ret.Timestamp;
                item.Message = ret.Message;

                // 当变量值发生变化时，通过 MQTT 推送消息（用于前端展示）
                if (JsonConvert.SerializeObject(item.Values[1]) != JsonConvert.SerializeObject(item.Values[0]) ||
                    JsonConvert.SerializeObject(item.CookedValues[1]) != JsonConvert.SerializeObject(item.CookedValues[0]))
                {
                    var msgInternal = new InjectedMqttApplicationMessage(
                        new MqttApplicationMessage()
                        {
                            Topic = $"internal/v1/gateway/telemetry/{Device.DeviceName}/{item.Name}",
                            PayloadSegment = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ret))
                        });
                    mqttServer.InjectApplicationMessage(msgInternal);
                }
                await Task.Delay((int)Device.CmdPeriod, token);
            }
        }

        public async void MyMqttClient_OnExcRpc(object? sender, RpcRequest e)
        {
            if (e.DeviceName == Device.DeviceName || Device.DeviceVariables.Select(x => x.Alias).Contains(e.DeviceName))
            {
                var rpcLog = new RpcLog
                {
                    DeviceId = Device.ID,
                    StartTime = DateTime.Now,
                    Method = e.Method,
                    RpcSide = RpcSide.ServerSide,
                    Params = JsonConvert.SerializeObject(e.Params)
                };

                _logger.LogInformation($"{e.DeviceName}收到RPC,{e}");
                var rpcResponse = new RpcResponse
                {
                    DeviceName = e.DeviceName,
                    RequestId = e.RequestId,
                    IsSuccess = false,
                    Method = e.Method
                };

                if (e.Method.ToLower() == "write")
                {
                    _resetEvent.Reset();
                    bool rpcConnected = false;
                    if (!Driver.IsConnected)
                    {
                        if (Driver.Connect())
                            rpcConnected = true;
                    }

                    if (Driver.IsConnected)
                    {
                        foreach (var para in e.Params)
                        {
                            DeviceVariable? deviceVariable;
                            if (e.DeviceName == Device.DeviceName)
                                deviceVariable = Device.DeviceVariables.FirstOrDefault(x => x.Name == para.Key);
                            else
                                deviceVariable = Device.DeviceVariables.FirstOrDefault(x => x.Name == para.Key && x.Alias == e.DeviceName);

                            if (deviceVariable != null && deviceVariable.ProtectType != ProtectTypeEnum.ReadOnly)
                            {
                                var ioArgModel = new DriverAddressIoArgModel
                                {
                                    Address = deviceVariable.DeviceAddress,
                                    Value = para.Value,
                                    ValueType = deviceVariable.DataType,
                                    EndianType = deviceVariable.EndianType
                                };
                                var writeResponse = await Driver.WriteAsync(e.RequestId, deviceVariable.Method, ioArgModel);
                                rpcResponse.IsSuccess = writeResponse.IsSuccess;
                                if (!writeResponse.IsSuccess)
                                    rpcResponse.Description += writeResponse.Description;
                            }
                            else
                            {
                                rpcResponse.IsSuccess = false;
                                rpcResponse.Description += $"未能找到支持写入的变量:{para.Key},";
                            }
                        }
                        if (rpcConnected)
                            Driver.Close();
                    }
                    else
                    {
                        rpcResponse.IsSuccess = false;
                        rpcResponse.Description = $"{e.DeviceName} 连接失败";
                    }
                    _resetEvent.Set();
                }
                else
                {
                    rpcResponse.IsSuccess = false;
                    rpcResponse.Description = $"方法:{e.Method}暂未实现";
                }

                await _messageService.ResponseRpcAsync(rpcResponse);
                rpcLog.IsSuccess = rpcResponse.IsSuccess;
                rpcLog.Description = rpcResponse.Description;
                rpcLog.EndTime = DateTime.Now;

                await using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
                dc.Set<RpcLog>().Add(rpcLog);
                await dc.SaveChangesAsync();
            }
        }

        public void StopThread()
        {
            _logger.LogInformation($"线程停止:{Device.DeviceName}");
            if (Device.DeviceVariables != null && Device.DeviceVariables.Any())
            {
                foreach (var group in Device.DeviceVariables.GroupBy(x => x.Alias))
                {
                    var deviceName = string.IsNullOrWhiteSpace(group.Key) ? Device.DeviceName : group.Key;
                    _messageService?.DeviceDisconnected(deviceName, Device);
                }
            }
            _messageService.OnExcRpc -= MyMqttClient_OnExcRpc;
            _tokenSource.Cancel();
            Driver.Close();
        }

        public void Dispose()
        {
            Driver.Dispose();
            _interpreter = null;
            MethodDelegates.Clear();
            _tokenSource.Dispose();
            _resetEvent.Dispose();
            _logger.LogInformation($"线程释放,{Device.DeviceName}");
        }

        // 处理 MySQL 转义符问题
        private string DealMysqlStr(string expression)
        {
            return expression.Replace("&lt;", "<")
                             .Replace("&gt;", ">")
                             .Replace("&amp;", "&")
                             .Replace("&quot;", "\"");
        }
    }
}
