using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using PluginInterface;
using System.Net;
using System.Reflection;
using WalkingTec.Mvvm.Core;

namespace Plugin
{
    public class DeviceService : IDisposable
    {
        private readonly ILogger<DeviceService> _logger;
        public DriverService DriverManager;

        public List<DeviceThread> DeviceThreads { get; } = new List<DeviceThread>();
        private readonly MessageService _messageService;
        private readonly MqttServer _mqttServer;
        private readonly string _connectSetting = IoTBackgroundService.connnectSetting;
        private readonly DBTypeEnum _dbType = IoTBackgroundService.DbType;

        public DeviceService(IConfiguration configRoot, DriverService driverManager, MessageService messageService,
            MqttServer mqttServer, ILogger<DeviceService> logger)
        {
            _logger = logger;
            DriverManager = driverManager;
            _messageService = messageService;
            _mqttServer = mqttServer ?? throw new ArgumentNullException(nameof(mqttServer));

            CreateDeviceThreads();
        }

        public void CreateDeviceThreads()
        {
            try
            {
                using (var dc = new DataContext(_connectSetting, _dbType))
                {
                    var devices = dc.Set<Device>()
                                    .Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                                    .Include(x => x.Parent)
                                    .Include(x => x.Driver)
                                    .Include(x => x.DeviceConfigs)
                                    .Include(x => x.DeviceVariables)
                                    .AsNoTracking()
                                    .ToList();
                    _logger.LogInformation($"Loaded Devices Count: {devices.Count}");
                    foreach (var device in devices)
                    {
                        CreateDeviceThread(device);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadDevicesError");
            }
        }

        public void UpdateDevice(Device device)
        {
            try
            {
                _logger.LogInformation($"UpdateDevice Start: {device.DeviceName}");
                RemoveDeviceThread(device);
                CreateDeviceThread(device);
                _logger.LogInformation($"UpdateDevice End: {device.DeviceName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateDevice Error: {device.DeviceName}");
            }
        }

        public void UpdateDevices(List<Device> devices)
        {
            foreach (var device in devices)
            {
                UpdateDevice(device);
            }
        }

        public void CreateDeviceThread(Device device)
        {
            try
            {
                _logger.LogInformation($"CreateDeviceThread Start: {device.DeviceName}");
                using (var dc = new DataContext(_connectSetting, _dbType))
                {
                    var systemConfig = dc.Set<SystemConfig>().FirstOrDefault();
                    if (systemConfig == null)
                    {
                        _logger.LogError("System configuration not found.");
                        return;
                    }

                    var driverInfo = DriverManager.DriverInfos
                        .SingleOrDefault(x => x.Type.FullName == device.Driver?.AssembleName);
                    if (driverInfo == null)
                    {
                        _logger.LogError($"Driver not found for device: [{device.DeviceName}], driver: [{device.Driver?.AssembleName}]");
                        return;
                    }

                    var settings = dc.Set<DeviceConfig>()
                                     .Where(x => x.DeviceId == device.ID)
                                     .AsNoTracking()
                                     .ToList();

                    // 创建驱动实例，使用 (string, ILogger) 构造函数
                    var constructor = driverInfo.Type.GetConstructor(new Type[] { typeof(string), typeof(ILogger) });
                    var driverInstance = constructor?.Invoke(new object[] { device.DeviceName, _logger }) as IDriver;
                    if (driverInstance == null)
                    {
                        _logger.LogError($"Failed to create driver instance for device: {device.DeviceName}");
                        return;
                    }

                    // 读取驱动类型中的所有带有 ConfigParameterAttribute 的属性，并赋值
                    foreach (var property in driverInfo.Type.GetProperties())
                    {
                        if (property.GetCustomAttribute<ConfigParameterAttribute>() is null)
                            continue;

                        var setting = settings.FirstOrDefault(x => x.DeviceConfigName == property.Name);
                        if (setting == null)
                            continue;

                        try
                        {
                            var convertedValue = ConvertSettingValue(property.PropertyType, setting.Value);
                            property.SetValue(driverInstance, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error setting property {property.Name} for device {device.DeviceName}");
                        }
                    }

                    var deviceThread = new DeviceThread(device, driverInstance, systemConfig.GatewayName,
                                                        _messageService, _mqttServer, _logger);
                    DeviceThreads.Add(deviceThread);
                }
                _logger.LogInformation($"CreateDeviceThread End: {device.DeviceName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateDeviceThread Error: {device.DeviceName}");
            }
        }

        /// <summary>
        /// 辅助方法：根据目标属性类型将字符串配置值转换为对应类型
        /// </summary>
        private object ConvertSettingValue(Type targetType, string value)
        {
            if (targetType == typeof(bool))
                return value != "0";
            if (targetType == typeof(byte))
                return byte.Parse(value);
            if (targetType == typeof(sbyte))
                return sbyte.Parse(value);
            if (targetType == typeof(short))
                return short.Parse(value);
            if (targetType == typeof(ushort))
                return ushort.Parse(value);
            if (targetType == typeof(int))
                return int.Parse(value);
            if (targetType == typeof(uint))
                return uint.Parse(value);
            if (targetType == typeof(long))
                return long.Parse(value);
            if (targetType == typeof(ulong))
                return ulong.Parse(value);
            if (targetType == typeof(float))
                return float.Parse(value);
            if (targetType == typeof(double))
                return double.Parse(value);
            if (targetType == typeof(decimal))
                return decimal.Parse(value);
            if (targetType == typeof(Guid))
                return Guid.Parse(value);
            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);
            if (targetType == typeof(string))
                return value;
            if (targetType == typeof(IPAddress))
                return IPAddress.Parse(value);
            if (targetType.IsEnum)
                return Enum.Parse(targetType, value);

            throw new NotSupportedException($"Type {targetType.FullName} is not supported for configuration conversion.");
        }

        public void CreateDeviceThreads(List<Device> devices)
        {
            foreach (var device in devices)
            {
                CreateDeviceThread(device);
            }
        }

        public void RemoveDeviceThread(Device device)
        {
            var deviceThread = DeviceThreads.FirstOrDefault(x => x.Device.ID == device.ID);
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
            {
                RemoveDeviceThread(device);
            }
        }

        public List<ComboSelectListItem> GetDriverMethods(Guid? deviceId)
        {
            var driverMethods = new List<ComboSelectListItem>();
            try
            {
                _logger.LogInformation($"GetDriverMethods Start: {deviceId}");
                var methodInfos = DeviceThreads.FirstOrDefault(x => x.Device.ID == deviceId)?.MethodDelegates.Keys;
                if (methodInfos != null)
                {
                    foreach (var method in methodInfos)
                    {
                        driverMethods.Add(new ComboSelectListItem
                        {
                            Text = method,
                            Value = method
                        });
                    }
                }
                _logger.LogInformation($"GetDriverMethods End: {deviceId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetDriverMethods Error: {deviceId}");
            }
            return driverMethods;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing DeviceService...");
            foreach (var thread in DeviceThreads)
            {
                thread.StopThread();
                thread.Dispose();
            }
            DeviceThreads.Clear();
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