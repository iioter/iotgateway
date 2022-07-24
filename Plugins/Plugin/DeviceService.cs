using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PluginInterface;
using System.Net;
using System.Reflection;
using WalkingTec.Mvvm.Core;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using DynamicExpresso;
using MQTTnet.Server;
using Microsoft.Extensions.Logging;

namespace Plugin
{
    public class DeviceService : IDisposable
    {
        private readonly ILogger<DeviceService> _logger;
        public DriverService _DrvierManager;

        public List<DeviceThread> DeviceThreads = new List<DeviceThread>();
        private MyMqttClient _MyMqttClient;
        private IMqttServer _MqttServer;
        private string connnectSetting = IoTBackgroundService.connnectSetting;
        private DBTypeEnum DBType = IoTBackgroundService.DBType;
        public DeviceService(IConfiguration ConfigRoot, DriverService drvierManager, MyMqttClient myMqttClient, UAService uAService, IMqttServer mqttServer, ILogger<DeviceService> logger)
        {
            _logger = logger;
            _DrvierManager = drvierManager;
            _MyMqttClient = myMqttClient;
            _MqttServer = mqttServer;
            try
            {
                using (var DC = new DataContext(connnectSetting, DBType))
                {
                    var Devices = DC.Set<Device>().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device).Include(x => x.Parent).Include(x => x.Driver).Include(x => x.DeviceConfigs).Include(x => x.DeviceVariables).AsNoTracking().ToList();
                    _logger.LogInformation($"Loaded Devices Count:{Devices.Count()}");
                    foreach (var Device in Devices)
                    {
                        CreateDeviceThread(Device);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"LoadDevicesError", ex);
            }
        }

        public void UpdateDevice(Device device)
        {
            try
            {
                _logger.LogInformation($"UpdateDevice Start:{device.DeviceName}");
                RemoveDeviceThread(device);
                CreateDeviceThread(device);
                _logger.LogInformation($"UpdateDevice End:{device.DeviceName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateDevice Error:{device.DeviceName}", ex);
            }

        }

        public void UpdateDevices(List<Device> devices)
        {
            foreach (var device in devices)
                UpdateDevice(device);
        }

        public void CreateDeviceThread(Device Device)
        {
            try
            {
                _logger.LogInformation($"CreateDeviceThread Start:{Device.DeviceName}");
                using (var DC = new DataContext(connnectSetting, DBType))
                {
                    var systemManage = DC.Set<SystemConfig>().FirstOrDefault();
                    var driver = _DrvierManager.DriverInfos.Where(x => x.Type.FullName == Device.Driver.AssembleName).SingleOrDefault();
                    if (driver == null)
                        _logger.LogError($"找不到设备:[{Device.DeviceName}]的驱动:[{Device.Driver.AssembleName}]");
                    else
                    {
                        var settings = DC.Set<DeviceConfig>().Where(x => x.DeviceId == Device.ID).AsNoTracking().ToList();
                        Type[] types = new Type[1] { typeof(Guid) };
                        object[] param = new object[1] { Device.ID };

                        ConstructorInfo constructor = driver.Type.GetConstructor(types);
                        var DeviceObj = constructor.Invoke(param) as IDriver;

                        foreach (var p in driver.Type.GetProperties())
                        {
                            var config = p.GetCustomAttribute(typeof(ConfigParameterAttribute));
                            var setting = settings.Where(x => x.DeviceConfigName == p.Name).FirstOrDefault();
                            if (config == null || setting == null)
                                continue;

                            object value = setting.Value;

                            if (p.PropertyType == typeof(bool))
                                value = setting.Value != "0";
                            else if (p.PropertyType == typeof(byte))
                                value = byte.Parse(setting.Value);
                            else if (p.PropertyType == typeof(sbyte))
                                value = sbyte.Parse(setting.Value);
                            else if (p.PropertyType == typeof(short))
                                value = short.Parse(setting.Value);
                            else if (p.PropertyType == typeof(ushort))
                                value = ushort.Parse(setting.Value);
                            else if (p.PropertyType == typeof(int))
                                value = int.Parse(setting.Value);
                            else if (p.PropertyType == typeof(uint))
                                value = uint.Parse(setting.Value);
                            else if (p.PropertyType == typeof(long))
                                value = long.Parse(setting.Value);
                            else if (p.PropertyType == typeof(ulong))
                                value = ulong.Parse(setting.Value);
                            else if (p.PropertyType == typeof(float))
                                value = float.Parse(setting.Value);
                            else if (p.PropertyType == typeof(double))
                                value = double.Parse(setting.Value);
                            else if (p.PropertyType == typeof(decimal))
                                value = decimal.Parse(setting.Value);
                            else if (p.PropertyType == typeof(Guid))
                                value = Guid.Parse(setting.Value);
                            else if (p.PropertyType == typeof(DateTime))
                                value = DateTime.Parse(setting.Value);
                            else if (p.PropertyType == typeof(string))
                                value = setting.Value;
                            else if (p.PropertyType == typeof(IPAddress))
                                value = IPAddress.Parse(setting.Value);
                            else if (p.PropertyType.BaseType == typeof(Enum))
                                value = Enum.Parse(p.PropertyType, setting.Value);

                            p.SetValue(DeviceObj, value);
                        }

                        var deviceThread = new DeviceThread(Device, DeviceObj, systemManage.GatewayName, _MyMqttClient, _MqttServer, _logger);
                        DeviceThreads.Add(deviceThread);
                    }


                }

                _logger.LogInformation($"CreateDeviceThread End:{Device.DeviceName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"CreateDeviceThread Error:{Device.DeviceName}", ex);

            }

        }

        public void CreateDeviceThreads(List<Device> devices)
        {
            foreach (Device device in devices)
                CreateDeviceThread(device);
        }

        public void RemoveDeviceThread(Device devices)
        {
            if (devices != null)
            {
                var DeviceThread = DeviceThreads.Where(x => x._device.ID == devices.ID).FirstOrDefault();
                if (DeviceThread != null)
                {
                    DeviceThread.StopThread();
                    DeviceThread.Dispose();
                    DeviceThreads.Remove(DeviceThread);
                }
            }
        }

        public void RemoveDeviceThreads(List<Device> devices)
        {
            foreach (var device in devices)
                RemoveDeviceThread(device);
        }

        public List<ComboSelectListItem> GetDriverMethods(Guid? deviceId)
        {
            List<ComboSelectListItem> driverFilesComboSelect = new List<ComboSelectListItem>();
            try
            {
                _logger.LogInformation($"GetDriverMethods Start:{deviceId}");
                foreach (var method in DeviceThreads.Where(x => x._device.ID == deviceId).FirstOrDefault()?.Methods)
                {
                    var Attribute = method.CustomAttributes.ToList().FirstOrDefault().ConstructorArguments;
                    var item = new ComboSelectListItem
                    {
                        Text = method.Name,
                        Value = method.Name,
                    };
                    driverFilesComboSelect.Add(item);
                }
                _logger.LogInformation($"GetDriverMethods End:{deviceId}");
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"GetDriverMethods Error:{deviceId}");
            }

            return driverFilesComboSelect;
        }
        public void Dispose()
        {
            _logger.LogInformation("Dispose");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
