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

namespace Plugin
{
    public class DeviceThread : IDisposable
    {
        public Device Device { get; set; }
        public IDriver Driver { get; set; }
        public Dictionary<Guid, DriverReturnValueModel> DeviceValues { get; set; } = new();
        internal List<MethodInfo> Methods { get; set; }
        private Task task { get; set; } = null;
        private DateTime TsStartDt = new DateTime(1970, 1, 1);
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Interpreter Interpreter = null;

        public DeviceThread(Device device, IDriver driver, string ProjectId, MyMqttClient myMqttClient, Interpreter interpreter, IMqttServer mqttServer)
        {
            Device = device;
            Driver = driver;
            Interpreter = interpreter;
            Methods = Driver.GetType().GetMethods().Where(x => x.GetCustomAttribute(typeof(MethodAttribute)) != null).ToList();
            if (Device.AutoStart)
            {
                Console.WriteLine($"采集线程已启动:{Device.DeviceName}");

                using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
                {
                    if (Device.DeviceVariables != null)
                    {
                        foreach (var item in Device.DeviceVariables)
                        {
                            DeviceValues[item.ID] = new() { StatusType = VaribaleStatusTypeEnum.Bad };
                        }
                    }
                }
                task = Task.Run(() =>
                {
                    while (true)
                    {
                        if (tokenSource.IsCancellationRequested)
                        {
                            Console.WriteLine($"{Device.DeviceName}，停止线程");
                            return;
                        }

                        try
                        {
                            Dictionary<string, List<PayLoad>> sendModel = new() { { Device.DeviceName, new() } };

                            var payLoad = new PayLoad() { Values = new() };
                            if (false)//Device.DeviceConfigs != null  配置数据先不上传
                                foreach (var DeviceConfig in Device.DeviceConfigs)
                                    payLoad.Values[DeviceConfig.DeviceConfigName] = DeviceConfig.Value;

                            if (driver.IsConnected)
                            {
                                if (Device.DeviceVariables != null)
                                {
                                    foreach (var item in Device.DeviceVariables)
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
                                            ret = (DriverReturnValueModel)method.Invoke(Driver, new object[1] { ioarg });

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
                                            mqttServer.PublishAsync($"internal/v1/gateway/telemetry/{Device.DeviceName}/{item.Name}", JsonConvert.SerializeObject(ret));
                                            //这是在线组态要用的
                                            mqttServer.PublishAsync($"v1/gateway/telemetry/{Device.DeviceName}/{item.Name}", JsonConvert.SerializeObject(ret.CookedValue));
                                        }

                                        DeviceValues[item.ID] = ret;

                                    }
                                    payLoad.TS = (long)(DateTime.Now - TsStartDt).TotalMilliseconds;

                                    if (DeviceValues.Any(x => x.Value.Value ==null))
                                    {
                                        payLoad.Values = null;
                                        payLoad.DeviceStatus = DeviceStatusTypeEnum.Bad;
                                    }
                                    else
                                    {
                                        payLoad.DeviceStatus = DeviceStatusTypeEnum.Good;
                                        sendModel[Device.DeviceName] = new List<PayLoad> { payLoad };
                                        myMqttClient.Publish(Device,sendModel);
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
                            Console.WriteLine($"线程循环异常,{Device.DeviceName},{ex}");
                        }

                        Thread.Sleep((int)Driver.MinPeriod);
                    }
                });
            }

        }

        public void StopThread()
        {
            if (task != null)
            {
                Driver.Close();
                tokenSource.Cancel();
            }
        }

        public void Dispose()
        {
            Driver.Dispose();
            Console.WriteLine($"{Device.DeviceName},释放");
        }

        //mysql会把一些符号转义，没找到原因，先临时处理下
        private string DealMysqlStr(string Expression)
        {
            return Expression.Replace("&lt;", ">").Replace("&gt;", "<").Replace("&amp;", "&");
        }
    }

}
