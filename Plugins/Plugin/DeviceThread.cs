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
        private readonly ILogger _logger;
        public readonly Device Device;
        public readonly IDriver Driver;
        private readonly string _projectId;
        private readonly MyMqttClient? _myMqttClient;
        private Interpreter? _interpreter;
        internal List<MethodInfo>? Methods { get; set; }
        private Task? _task;
        private readonly DateTime _tsStartDt = new(1970, 1, 1);
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new();

        public DeviceThread(Device device, IDriver driver, string projectId, MyMqttClient myMqttClient,
            MqttServer mqttServer, ILogger logger)
        {
            _myMqttClient = myMqttClient;
            _myMqttClient.OnExcRpc += MyMqttClient_OnExcRpc;
            Device = device;
            Driver = driver;
            _projectId = projectId;
            _interpreter = new Interpreter();
            _logger = logger;
            Methods = Driver.GetType().GetMethods().Where(x => x.GetCustomAttribute(typeof(MethodAttribute)) != null)
                .ToList();
            if (Device.AutoStart)
            {
                _logger.LogInformation($"线程已启动:{Device.DeviceName}");

                if (Device.DeviceVariables != null)
                {
                    foreach (var item in Device.DeviceVariables)
                        item.StatusType = VaribaleStatusTypeEnum.Bad;
                }

                _task = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(5000);
                    //上传客户端属性
                    foreach (var deviceVariables in Device.DeviceVariables!.GroupBy(x => x.Alias))
                    {
                        myMqttClient.UploadAttributeAsync(string.IsNullOrWhiteSpace(deviceVariables.Key)
                                ? Device.DeviceName
                                : deviceVariables.Key,
                            device.DeviceConfigs.Where(x => x.DataSide == DataSide.ClientSide)
                                .ToDictionary(x => x.DeviceConfigName, x => x.Value));
                    }

                    while (true)
                    {
                        if (_tokenSource.IsCancellationRequested)
                        {
                            _logger.LogInformation($"停止线程:{Device.DeviceName}");
                            return;
                        }

                        lock (_lock)
                        {
                            try
                            {
                                if (driver.IsConnected)
                                {
                                    foreach (var deviceVariables in Device.DeviceVariables!.GroupBy(x => x.Alias))
                                    {
                                        string deviceName = string.IsNullOrWhiteSpace(deviceVariables.Key)
                                            ? Device.DeviceName
                                            : deviceVariables.Key;

                                        Dictionary<string, List<PayLoad>> sendModel = new()
                                            { { deviceName, new() } };
                                        var payLoad = new PayLoad() { Values = new() };

                                        if (deviceVariables.Any())
                                        {
                                            foreach (var item in deviceVariables.OrderBy(x => x.Index))
                                            {
                                                Thread.Sleep((int)Device.CmdPeriod);

                                                var ret = new DriverReturnValueModel();
                                                var ioarg = new DriverAddressIoArgModel
                                                {
                                                    ID = item.ID,
                                                    Address = item.DeviceAddress,
                                                    ValueType = item.DataType,
                                                    EndianType = item.EndianType
                                                };
                                                var method = Methods.FirstOrDefault(x => x.Name == item.Method);
                                                if (method == null)
                                                    ret.StatusType = VaribaleStatusTypeEnum.MethodError;
                                                else
                                                    ret = (DriverReturnValueModel)method.Invoke(Driver,
                                                        new object[] { ioarg })!;

                                                if (ret.StatusType == VaribaleStatusTypeEnum.Good &&
                                                    !string.IsNullOrWhiteSpace(item.Expressions?.Trim()))
                                                {
                                                    try
                                                    {
                                                        ret.CookedValue = _interpreter.Eval(DealMysqlStr(item.Expressions)
                                                            .Replace("raw", ret.Value?.ToString()));
                                                    }
                                                    catch (Exception)
                                                    {
                                                        ret.StatusType = VaribaleStatusTypeEnum.ExpressionError;
                                                    }
                                                }
                                                else
                                                    ret.CookedValue = ret.Value;


                                                if(item.IsUpload)
                                                    payLoad.Values[item.Name] = ret.CookedValue;

                                                ret.VarId = item.ID;

                                                //变化了才推送到mqttserver，用于前端展示
                                                if (item.StatusType != ret.StatusType ||
                                                    item.Value?.ToString() != ret.Value?.ToString() ||
                                                    item.CookedValue?.ToString() != ret.CookedValue?.ToString())
                                                {
                                                    //这是设备变量列表要用的
                                                    var msgInternal = new InjectedMqttApplicationMessage(
                                                        new MqttApplicationMessage()
                                                        {
                                                            Topic = $"internal/v1/gateway/telemetry/{deviceName}/{item.Name}",
                                                            Payload = Encoding.UTF8.GetBytes(JsonUtility.SerializeToJson(ret))
                                                        });
                                                    mqttServer.InjectApplicationMessage(msgInternal);
                                                    //这是在线组态要用的
                                                    var msgConfigure = new InjectedMqttApplicationMessage(
                                                        new MqttApplicationMessage()
                                                        {
                                                            Topic = $"v1/gateway/telemetry/{deviceName}/{item.Name}",
                                                            Payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ret.CookedValue))
                                                        });
                                                    mqttServer.InjectApplicationMessage(msgConfigure);
                                                }

                                                item.Value = ret.Value;
                                                item.CookedValue = ret.CookedValue;
                                                item.Timestamp = ret.Timestamp;
                                                item.StatusType = ret.StatusType;
                                            }

                                            payLoad.TS = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds;

                                            if (deviceVariables.All(x => x.StatusType == VaribaleStatusTypeEnum.Good))
                                            {
                                                payLoad.DeviceStatus = DeviceStatusTypeEnum.Good;
                                                sendModel[deviceName] = new List<PayLoad> { payLoad };
                                                myMqttClient
                                                    .PublishTelemetryAsync(deviceName,
                                                        Device, sendModel).Wait();
                                            }
                                            else if (deviceVariables.Any(x => x.StatusType == VaribaleStatusTypeEnum.Bad))
                                                _myMqttClient?.DeviceDisconnected(deviceName, Device);
                                        }

                                    }

                                    //只要有读取异常且连接正常就断开
                                    if (Device.DeviceVariables!.Any(x => x.StatusType != VaribaleStatusTypeEnum.Good) && driver.IsConnected)
                                    {
                                        driver.Close();
                                        driver.Dispose();
                                    }
                                }
                                else
                                {
                                    foreach (var deviceVariables in Device.DeviceVariables!.GroupBy(x => x.Alias))
                                    {
                                        string deviceName = string.IsNullOrWhiteSpace(deviceVariables.Key)
                                            ? Device.DeviceName
                                            : deviceVariables.Key;

                                        _myMqttClient?.DeviceDisconnected(deviceName, Device);
                                    }

                                    if (driver.Connect())
                                    {
                                        foreach (var deviceVariables in Device.DeviceVariables!.GroupBy(x => x.Alias))
                                        {
                                            string deviceName = string.IsNullOrWhiteSpace(deviceVariables.Key)
                                                ? Device.DeviceName
                                                : deviceVariables.Key;

                                            _myMqttClient?.DeviceConnected(deviceName, Device);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"线程循环异常,{Device.DeviceName}");
                            }
                        }


                        Thread.Sleep(Device.DeviceVariables!.Any() ? (int)Driver.MinPeriod : 10000);
                    }
                }, TaskCreationOptions.LongRunning);
            }
            //else
            //_myMqttClient?.DeviceDisconnected(Device);
        }

        public void MyMqttClient_OnExcRpc(object? sender, RpcRequest e)
        {
            //设备名或者设备别名
            if (e.DeviceName == Device.DeviceName || Device.DeviceVariables.Select(x => x.Alias).Contains(e.DeviceName))
            {
                RpcLog rpcLog = new RpcLog()
                {
                    DeviceId = Device.ID,
                    StartTime = DateTime.Now,
                    Method = e.Method,
                    RpcSide = RpcSide.ServerSide,
                    Params = JsonConvert.SerializeObject(e.Params)
                };

                _logger.LogInformation($"{e.DeviceName}收到RPC,{e}");
                RpcResponse rpcResponse = new()
                    { DeviceName = e.DeviceName, RequestId = e.RequestId, IsSuccess = false, Method = e.Method };
                //执行写入变量RPC
                if (e.Method.ToLower() == "write")
                {
                    lock (_lock)
                    {
                        bool rpcConnected = false;
                        //没连接就连接
                        if (!Driver.IsConnected)
                            if (Driver.Connect())
                                rpcConnected = true;

                        //连接成功就尝试一个一个的写入，注意:目前写入地址和读取地址是相同的，对于PLC来说没问题，其他的要自己改........
                        if (Driver.IsConnected)
                        {
                            foreach (var para in e.Params)
                            {
                                //先查配置项，要用到配置的地址、数据类型、方法(方法最主要是用于区分写入数据的辅助判断，比如modbus不同的功能码)
                                //先找别名中的变量名，找不到就用设备名
                                DeviceVariable? deviceVariable;
                                if (e.DeviceName == Device.DeviceName)
                                    deviceVariable = Device.DeviceVariables.FirstOrDefault(x =>
                                        x.Name == para.Key && string.IsNullOrWhiteSpace(x.Alias));
                                else
                                    deviceVariable = Device.DeviceVariables.FirstOrDefault(x =>
                                        x.Name == para.Key && x.Alias == e.DeviceName);

                                if (deviceVariable != null)
                                {
                                    DriverAddressIoArgModel ioArgModel = new()
                                    {
                                        Address = deviceVariable.DeviceAddress,
                                        Value = para.Value,
                                        ValueType = deviceVariable.DataType
                                    };
                                    var writeResponse = Driver
                                        .WriteAsync(e.RequestId, deviceVariable.Method, ioArgModel).Result;
                                    rpcResponse.IsSuccess = writeResponse.IsSuccess;
                                    if (!writeResponse.IsSuccess)
                                    {
                                        rpcResponse.Description += writeResponse.Description;
                                    }
                                }
                                else
                                {
                                    rpcResponse.IsSuccess = false;
                                    rpcResponse.Description += $"未能找到变量:{para.Key},";
                                }
                            }

                            if (rpcConnected)
                                Driver.Close();
                        }
                        else //连接失败
                        {
                            rpcResponse.IsSuccess = false;
                            rpcResponse.Description = $"{e.DeviceName} 连接失败";
                        }
                    }
                }
                //其他RPC TODO
                else
                {
                    rpcResponse.IsSuccess = false;
                    rpcResponse.Description = $"方法:{e.Method}暂未实现";
                }

                //反馈RPC
                _myMqttClient.ResponseRpcAsync(rpcResponse).Wait();
                //纪录入库
                rpcLog.IsSuccess = rpcResponse.IsSuccess;
                rpcLog.Description = rpcResponse.Description;
                rpcLog.EndTime = DateTime.Now;


                using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
                dc.Set<RpcLog>().Add(rpcLog);
                dc.SaveChanges();
            }
        }

        public void StopThread()
        {
            _logger.LogInformation($"线程停止:{Device.DeviceName}");
            if (Device.DeviceVariables!=null&& Device.DeviceVariables.Any())
            {
                foreach (var deviceVariables in Device.DeviceVariables.GroupBy(x => x.Alias))
                {
                    string deviceName = string.IsNullOrWhiteSpace(deviceVariables.Key)
                        ? Device.DeviceName
                        : deviceVariables.Key;
                    _myMqttClient?.DeviceDisconnected(deviceName, Device);
                }
            }

            if (_task == null) return;
            if (_myMqttClient != null) _myMqttClient.OnExcRpc -= MyMqttClient_OnExcRpc;
            _tokenSource.Cancel();
            Driver.Close();
        }

        public void Dispose()
        {
            Driver.Dispose();
            _interpreter = null;
            Methods = null;
            _logger.LogInformation($"线程释放,{Device.DeviceName}");
        }

        //mysql会把一些符号转义，没找到原因，先临时处理下
        private string DealMysqlStr(string expression)
        {
            return expression.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&quot;", "\"");
        }
    }
}