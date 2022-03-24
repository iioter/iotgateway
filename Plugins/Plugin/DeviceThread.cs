using Microsoft.EntityFrameworkCore;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using WalkingTec.Mvvm.Core;
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
        public Dictionary<Guid, DriverReturnValueModel> DeviceValues { get; set; } = new();
        internal List<MethodInfo> Methods { get; set; }
        private Task task { get; set; } = null;
        private DateTime TsStartDt = new DateTime(1970, 1, 1);
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Interpreter Interpreter = null;

        public DeviceThread(Device device, IDriver driver, string ProjectId, MyMqttClient myMqttClient, Interpreter interpreter, IMqttServer mqttServer, ILogger logger)
        {
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
                myMqttClient.UploadAttributeAsync(device.DeviceName, device.DeviceConfigs.Where(x => x.DataSide == DataSide.ClientSide).ToDictionary(x => x.DeviceConfigName, x => x.Value));

                task = Task.Run(() =>
                 {
                     while (true)
                     {
                         if (tokenSource.IsCancellationRequested)
                         {
                             _logger.LogInformation($"停止线程:{_device.DeviceName}");
                             return;
                         }

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
                                     payLoad.TS = (long)(DateTime.Now - TsStartDt).TotalMilliseconds;

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
                                 driver.Connect();
                             }
                         }
                         catch (Exception ex)
                         {
                             _logger.LogError($"线程循环异常,{_device.DeviceName}", ex);
                         }

                         Thread.Sleep((int)_driver.MinPeriod);
                     }
                 });
            }

        }

        public void StopThread()
        {
            _logger.LogInformation($"线程停止:{_device.DeviceName}");
            if (task != null)
            {
                _driver.Close();
                tokenSource.Cancel();
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
