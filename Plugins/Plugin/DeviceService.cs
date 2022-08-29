using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PluginInterface;
using System.Net;
using System.Reflection;
using WalkingTec.Mvvm.Core;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using MQTTnet.Server;
using Microsoft.Extensions.Logging;

namespace Plugin
{
    public class DeviceService : IDisposable
    {
        private readonly ILogger<DeviceService> _logger;
        public DriverService DrvierManager;

        public List<DeviceThread> DeviceThreads = new List<DeviceThread>();
        private readonly MyMqttClient _myMqttClient;
        private readonly UAService _uAService;
        private readonly MqttServer _mqttServer;
        private readonly string _connnectSetting = IoTBackgroundService.connnectSetting;
        private readonly DBTypeEnum _dbType = IoTBackgroundService.DbType;

        //UAService? uAService, 
        public DeviceService(IConfiguration configRoot, DriverService drvierManager, MyMqttClient myMqttClient,
            MqttServer mqttServer, ILogger<DeviceService> logger)
        {
            _logger = logger;
            DrvierManager = drvierManager;
            _myMqttClient = myMqttClient;
            //_uAService = uAService;
            _mqttServer = mqttServer ?? throw new ArgumentNullException(nameof(mqttServer));
            try
            {
                using (var dc = new DataContext(_connnectSetting, _dbType))
                {
                    var devices = dc.Set<Device>().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                        .Include(x => x.Parent).Include(x => x.Driver).Include(x => x.DeviceConfigs)
                        .Include(x => x.DeviceVariables).AsNoTracking().ToList();
                    _logger.LogInformation($"Loaded Devices Count:{devices.Count()}");
                    foreach (var device in devices)
                    {
                        CreateDeviceThread(device);
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

        public void CreateDeviceThread(Device device)
        {
            try
            {
                _logger.LogInformation($"CreateDeviceThread Start:{device.DeviceName}");
                using (var dc = new DataContext(_connnectSetting, _dbType))
                {
                    var systemManage = dc.Set<SystemConfig>().FirstOrDefault();
                    var driver = DrvierManager.DriverInfos
                        .SingleOrDefault(x => x.Type.FullName == device.Driver.AssembleName);
                    if (driver == null)
                        _logger.LogError($"找不到设备:[{device.DeviceName}]的驱动:[{device.Driver.AssembleName}]");
                    else
                    {
                        var settings = dc.Set<DeviceConfig>().Where(x => x.DeviceId == device.ID).AsNoTracking()
                            .ToList();

                        Type[] types = new Type[] { typeof(string), typeof(ILogger) };
                        object[] param = new object[] { device.DeviceName, _logger };

                        ConstructorInfo? constructor = driver.Type.GetConstructor(types);
                        var deviceObj = constructor?.Invoke(param) as IDriver;

                        foreach (var p in driver.Type.GetProperties())
                        {
                            var config = p.GetCustomAttribute(typeof(ConfigParameterAttribute));
                            var setting = settings.FirstOrDefault(x => x.DeviceConfigName == p.Name);
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

                            p.SetValue(deviceObj, value);
                        }

                        if (deviceObj != null && systemManage != null)
                        {
                            var deviceThread = new DeviceThread(device, deviceObj, systemManage.GatewayName,
                                _myMqttClient,
                                _mqttServer, _logger);
                            DeviceThreads.Add(deviceThread);
                        }
                    }
                }

                _logger.LogInformation($"CreateDeviceThread End:{device.DeviceName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"CreateDeviceThread Error:{device.DeviceName}", ex);
            }
        }

        public void CreateDeviceThreads(List<Device> devices)
        {
            foreach (Device device in devices)
                CreateDeviceThread(device);
        }

        public void RemoveDeviceThread(Device devices)
        {
            var deviceThread = DeviceThreads.FirstOrDefault(x => x.Device.ID == devices.ID);
            if (deviceThread != null)
            {
                deviceThread.StopThread();
                deviceThread.Dispose();
                DeviceThreads.Remove(deviceThread);
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
                var methodInfos = DeviceThreads.FirstOrDefault(x => x.Device.ID == deviceId)?.Methods;
                if (methodInfos != null)
                    foreach (var method in methodInfos)
                    {
                        var attribute = method.CustomAttributes.ToList().FirstOrDefault()?.ConstructorArguments;
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
                _logger.LogError($"GetDriverMethods Error:{deviceId}", ex);
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