using PluginInterface;
using System.Reflection;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using DynamicExpresso;
using MQTTnet.Server;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Plugin
{
    public class DeviceThread : IDisposable
    {
        private readonly ILogger _logger;
        public readonly Device _device;
        public readonly IDriver _driver;
        private readonly MyMqttClient _myMqttClient;
        public Dictionary<Guid, DriverReturnValueModel> DeviceValues { get; set; } = new();
        internal List<MethodInfo> Methods { get; set; }
        private Task task { get; set; } = null;
        private DateTime TsStartDt = new DateTime(1970, 1, 1);
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Interpreter Interpreter = null;
        private object _lock = new object();
        private bool lastConnected = false;
        public DeviceThread(Device device, IDriver driver, string ProjectId, MyMqttClient myMqttClient, Interpreter interpreter, IMqttServer mqttServer, ILogger logger)
        {
            _myMqttClient = myMqttClient;
            _myMqttClient.OnExcRpc += MyMqttClient_OnExcRpc;
            _device = device;
            _driver = driver;
            Interpreter = interpreter;
            _logger = logger;
            Methods = _driver.GetType().GetMethods().Where(x => x.GetCustomAttribute(typeof(MethodAttribute)) != null).ToList();
            if (_device.AutoStart)
            {
                _logger.LogInformation($"线程已启动:{_device.DeviceName}");

                using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
                {
                    if (_device.DeviceVariables != null)
                    {
                        foreach (var item in _device.DeviceVariables)
                        {
                            DeviceValues[item.ID] = new() { StatusType = VaribaleStatusTypeEnum.Bad };
                        }
                    }
                }

                task = Task.Run(() =>
                {
                    Thread.Sleep(5000);//上传客户端属性
                    myMqttClient.UploadAttributeAsync(device.DeviceName, device.DeviceConfigs.Where(x => x.DataSide == DataSide.ClientSide).ToDictionary(x => x.DeviceConfigName, x => x.Value));

                    while (true)
                    {
                        if (tokenSource.IsCancellationRequested)
                        {
                            _logger.LogInformation($"停止线程:{_device.DeviceName}");
                            return;
                        }

                        lock (_lock)
                        {
                            try
                            {
                                Dictionary<string, List<PayLoad>> sendModel = new() { { _device.DeviceName, new() } };

                                var payLoad = new PayLoad() { Values = new() };

                                if (driver.IsConnected)
                                {
                                    if (_device.DeviceVariables != null)
                                    {
                                        foreach (var item in _device.DeviceVariables)
                                        {
                                            var ret = new DriverReturnValueModel();
                                            var ioarg = new DriverAddressIoArgModel
                                            {
                                                ID = item.ID,
                                                Address = item.DeviceAddress,
                                                ValueType = item.DataType
                                            };
                                            var method = Methods.Where(x => x.Name == item.Method).FirstOrDefault();
                                            if (method == null)
                                                ret.StatusType = VaribaleStatusTypeEnum.MethodError;
                                            else
                                                ret = (DriverReturnValueModel)method.Invoke(_driver, new object[1] { ioarg });

                                            if (ret.StatusType == VaribaleStatusTypeEnum.Good && !string.IsNullOrWhiteSpace(item.Expressions?.Trim()))
                                            {
                                                try
                                                {
                                                    ret.CookedValue = interpreter.Eval(DealMysqlStr(item.Expressions).Replace("raw", ret.Value?.ToString()));
                                                }
                                                catch (Exception)
                                                {
                                                    ret.StatusType = VaribaleStatusTypeEnum.ExpressionError;
                                                }
                                            }
                                            else
                                                ret.CookedValue = ret.Value;

                                            payLoad.Values[item.Name] = ret.CookedValue;

                                            ret.VarId = item.ID;

                                            //变化了才推送到mqttserver，用于前端展示
                                            if (DeviceValues[item.ID].StatusType != ret.StatusType || DeviceValues[item.ID].Value?.ToString() != ret.Value?.ToString() || DeviceValues[item.ID].CookedValue?.ToString() != ret.CookedValue?.ToString())
                                            {
                                                //这是设备变量列表要用的
                                                mqttServer.PublishAsync($"internal/v1/gateway/telemetry/{_device.DeviceName}/{item.Name}", JsonConvert.SerializeObject(ret));
                                                //这是在线组态要用的
                                                mqttServer.PublishAsync($"v1/gateway/telemetry/{_device.DeviceName}/{item.Name}", JsonConvert.SerializeObject(ret.CookedValue));
                                            }

                                            DeviceValues[item.ID] = ret;

                                        }
                                        payLoad.TS = (long)(DateTime.UtcNow - TsStartDt).TotalMilliseconds;

                                        if (DeviceValues.Any(x => x.Value.Value == null))
                                        {
                                            payLoad.Values = null;
                                            payLoad.DeviceStatus = DeviceStatusTypeEnum.Bad;
                                        }
                                        else
                                        {
                                            payLoad.DeviceStatus = DeviceStatusTypeEnum.Good;
                                            sendModel[_device.DeviceName] = new List<PayLoad> { payLoad };
                                            myMqttClient.PublishTelemetry(_device, sendModel);
                                        }
                                    }

                                }
                                else
                                {
                                    if (driver.Connect())
                                    {
                                        lastConnected = true;
                                        _myMqttClient.DeviceConnected(_device.DeviceName);
                                    }
                                    else if (lastConnected)
                                    {
                                        lastConnected = false;
                                        _myMqttClient.DeviceDisconnected(_device.DeviceName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"线程循环异常,{_device.DeviceName}", ex);
                            }
                        }


                        Thread.Sleep((int)_driver.MinPeriod);
                    }
                });
            }
            else
                _myMqttClient.DeviceDisconnected(_device.DeviceName);
        }

        public void MyMqttClient_OnExcRpc(object? sender, RpcRequest e)
        {
            if (e.DeviceName == _device.DeviceName)
            {
                RpcLog rpcLog = new RpcLog()
                {
                    DeviceId = _device.ID,
                    StartTime = DateTime.Now,
                    Method = e.Method,
                    RpcSide = RpcSide.ServerSide,
                    Params = JsonConvert.SerializeObject(e.Params)
                };

                _logger.LogInformation($"{_device.DeviceName}收到RPC,{e}");
                RpcResponse rpcResponse = new() { DeviceName = e.DeviceName, RequestId = e.RequestId, IsSuccess = false };
                //执行写入变量RPC
                if (e.Method.ToLower() == "write")
                {
                    lock (_lock)
                    {
                        bool RpcConnected = false;
                        //没连接就连接
                        if (!_driver.IsConnected)
                            if (_driver.Connect())
                                RpcConnected = true;

                        //连接成功就尝试一个一个的写入，注意:目前写入地址和读取地址是相同的，对于PLC来说没问题，其他的要自己改........
                        if (_driver.IsConnected)
                        {
                            foreach (var para in e.Params)
                            {
                                //先查配置项，要用到配置的地址、数据类型、方法(方法最主要是用于区分写入数据的辅助判断，比如modbus不同的功能码)
                                var deviceVariable = _device.DeviceVariables.Where(x => x.Name == para.Key).FirstOrDefault();
                                if (deviceVariable != null)
                                {
                                    DriverAddressIoArgModel ioArgModel = new()
                                    {
                                        Address = deviceVariable.DeviceAddress,
                                        Value = para.Value,
                                        ValueType = deviceVariable.DataType
                                    };
                                    var writeResponse = _driver.WriteAsync(e.RequestId, deviceVariable.Method, ioArgModel).Result;
                                    rpcResponse.IsSuccess = writeResponse.IsSuccess;
                                    if (!writeResponse.IsSuccess)
                                    {
                                        rpcResponse.Description = writeResponse.Description;
                                        break;
                                    }
                                }
                                else
                                {
                                    rpcResponse.IsSuccess = false;
                                    rpcResponse.Description = $"未能找到变量:{para.Key}";
                                    break;
                                }
                            }
                            if (RpcConnected)
                                _driver.Close();

                        }
                        else//连接失败
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
                _myMqttClient.ResponseRpc(rpcResponse);
                //纪录入库
                rpcLog.IsSuccess = rpcResponse.IsSuccess;
                rpcLog.Description = rpcResponse.Description;
                rpcLog.EndTime = DateTime.Now;


                using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
                {
                    DC.Set<RpcLog>().Add(rpcLog);
                    DC.SaveChanges();
                }
            }
        }

        public void StopThread()
        {
            _logger.LogInformation($"线程停止:{_device.DeviceName}");
            _myMqttClient.DeviceDisconnected(_device.DeviceName);
            if (task != null)
            {
                _myMqttClient.OnExcRpc -= MyMqttClient_OnExcRpc;
                tokenSource.Cancel();
                _driver.Close();
            }
        }

        public void Dispose()
        {
            _driver.Dispose();
            _logger.LogInformation($"线程释放,{_device.DeviceName}");
        }

        //mysql会把一些符号转义，没找到原因，先临时处理下
        private string DealMysqlStr(string Expression)
        {
            return Expression.Replace("&lt;", ">").Replace("&gt;", "<").Replace("&amp;", "&");
        }
    }

}
