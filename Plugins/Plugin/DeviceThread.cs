using DynamicExpresso;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using PluginInterface;
using System.Reflection;
using System.Text;

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

        //增加时间容忍断开连接
        private DateTime _lastGoodTime = DateTime.UtcNow;

        private readonly TimeSpan _maxFailureDuration = TimeSpan.FromSeconds(60);

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

            // 订阅驱动数据上报事件
            Driver.OnDataReceived += OnDriverDataReceived;
            
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

        /// <summary>
/// 处理驱动主动上报的数据
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private async Task OnDriverDataReceived(object sender, DataReportEventArgs e)
{
    try
    {
        // 根据变量名或地址查找对应的DeviceVariable
        var deviceVariable = Device.DeviceVariables?.FirstOrDefault(v => 
            v.Name == e.VariableName || 
            v.DeviceAddress == e.Address ||
            (!string.IsNullOrEmpty(e.Address) && v.DeviceAddress == e.Address));

        if (deviceVariable == null)
        {
            _logger.LogWarning($"未找到对应的设备变量: {e.VariableName}, 地址: {e.Address}");
            return;
        }

        // 创建返回值模型
        var ret = new DriverReturnValueModel
        {
            Value = e.Value,
            StatusType = e.StatusType,
            Timestamp = e.Timestamp,
            Message = e.Message,
            VarId = deviceVariable.ID
        };

        // 将原始值加入队列（保存最近3次）
        deviceVariable.EnqueueVariable(ret.Value);

        // 表达式计算（如果配置了）
        if (ret.StatusType == VaribaleStatusTypeEnum.Good && !string.IsNullOrWhiteSpace(deviceVariable.Expressions?.Trim()))
        {
            var expressionText = DealMysqlStr(deviceVariable.Expressions)
                .Replace("raw",
                    deviceVariable.Values[0] is bool
                        ? $"Convert.ToBoolean(\"{deviceVariable.Values[0]}\")"
                        : deviceVariable.Values[0]?.ToString())
                .Replace("$v",
                    deviceVariable.Values[0] is bool
                        ? $"Convert.ToBoolean(\"{deviceVariable.Values[0]}\")"
                        : deviceVariable.Values[0]?.ToString())
                .Replace("$pv",
                    deviceVariable.Values[1] is bool
                        ? $"Convert.ToBoolean(\"{deviceVariable.Values[1]}\")"
                        : deviceVariable.Values[1]?.ToString())
                .Replace("$ppv",
                    deviceVariable.Values[2] is bool
                        ? $"Convert.ToBoolean(\"{deviceVariable.Values[2]}\")"
                        : deviceVariable.Values[2]?.ToString());

            try
            {
                ret.CookedValue = _interpreter!.Eval(expressionText);
            }
            catch (Exception ex)
            {
                ret.Message = $"表达式错误：{expressionText}";
                ret.StatusType = VaribaleStatusTypeEnum.ExpressionError;
                _logger.LogError(ex, $"表达式计算失败: {expressionText}");
            }
        }
        else
        {
            ret.CookedValue = ret.Value;
        }

        // 将计算后的值加入队列
        deviceVariable.EnqueueCookedVariable(ret.CookedValue);

        // 更新设备变量的状态
        deviceVariable.Value = ret.Value;
        deviceVariable.CookedValue = ret.CookedValue;
        deviceVariable.StatusType = ret.StatusType;
        deviceVariable.Timestamp = ret.Timestamp;
        deviceVariable.Message = ret.Message;

        // 发布到MQTT（用于前端展示）
        if (JsonConvert.SerializeObject(deviceVariable.Values[1]) != JsonConvert.SerializeObject(deviceVariable.Values[0]) ||
            JsonConvert.SerializeObject(deviceVariable.CookedValues[1]) != JsonConvert.SerializeObject(deviceVariable.CookedValues[0]))
        {
            var msgInternal = new InjectedMqttApplicationMessage(
                new MqttApplicationMessage()
                {
                    Topic = $"internal/v1/gateway/telemetry/{Device.DeviceName}/{deviceVariable.Name}",
                    PayloadSegment = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ret))
                });
            _mqttServer.InjectApplicationMessage(msgInternal);
        }

        // 如果变量配置为上传，则发布遥测数据
        if (deviceVariable.IsUpload && ret.StatusType == VaribaleStatusTypeEnum.Good)
        {
            var deviceName = string.IsNullOrWhiteSpace(deviceVariable.Alias) ? Device.DeviceName : deviceVariable.Alias;
            var sendModel = new Dictionary<string, List<PayLoad>>
            {
                {
                    deviceName,
                    new List<PayLoad>
                    {
                        new PayLoad
                        {
                            Values = new Dictionary<string, object> { { deviceVariable.Name, ret.CookedValue } },
                            TS = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds,
                            DeviceStatus = DeviceStatusTypeEnum.Good
                        }
                    }
                }
            };

            await _messageService.PublishTelemetryAsync(deviceName, Device, sendModel);
        }

        _logger.LogDebug($"处理驱动上报数据: {deviceVariable.Name} = {ret.CookedValue}");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"处理驱动上报数据失败: {e.VariableName}");
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
                            if (Device.DeviceVariables.All(x => x.StatusType != VaribaleStatusTypeEnum.Good))
                            {
                                // 超过设定时间未有任何一次读取成功，则认为死连接
                                if (DateTime.UtcNow - _lastGoodTime > _maxFailureDuration && Driver.IsConnected)
                                {
                                    _logger.LogWarning($"{Device.DeviceName} 读取变量连续失败超过 {_maxFailureDuration.TotalSeconds} 秒，主动断开连接重连");
                                    Driver.Close();
                                    Driver.Dispose();
                                }
                            }
                            else
                            {
                                // 至少有一个变量读取成功，刷新最后成功时间
                                _lastGoodTime = DateTime.UtcNow;
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
                        .Replace("raw",
                            item.Values[0] is bool
                                ? $"Convert.ToBoolean(\"{item.Values[0]}\")"
                                : item.Values[0]?.ToString())
                        .Replace("$v",
                            item.Values[0] is bool
                                ? $"Convert.ToBoolean(\"{item.Values[0]}\")"
                                : item.Values[0]?.ToString())
                        .Replace("$pv",
                            item.Values[1] is bool
                                ? $"Convert.ToBoolean(\"{item.Values[1]}\")"
                                : item.Values[1]?.ToString())
                        .Replace("$ppv",
                            item.Values[2] is bool
                                ? $"Convert.ToBoolean(\"{item.Values[2]}\")"
                                : item.Values[2]?.ToString());

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
            // 取消订阅驱动数据上报事件
            Driver.OnDataReceived -= OnDriverDataReceived;
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
