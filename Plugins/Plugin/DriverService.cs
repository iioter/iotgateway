using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PluginInterface;
using System.Reflection;
using System.Text.Json;
using WalkingTec.Mvvm.Core;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.Extensions.Logging;

namespace Plugin
{
    public class DriverService
    {
        private readonly ILogger<DriverService> _logger;
        private readonly string _driverPath;
        private readonly string[] _driverFiles;
        private readonly List<DriverInfo> _driverInfos = new();

        // 通过只读属性提供DriverInfos，防止外部修改
        public IReadOnlyList<DriverInfo> DriverInfos => _driverInfos.AsReadOnly();

        public DriverService(IConfiguration configRoot, ILogger<DriverService> logger)
        {
            _logger = logger;
            _driverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drivers/net6.0");

            try
            {
                _logger.LogInformation("LoadDriverFiles Start");
                _driverFiles = Directory.GetFiles(_driverPath, "*.dll");
                _logger.LogInformation($"LoadDriverFiles End, Count: {_driverFiles.Length}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadDriverFiles Error");
                _driverFiles = Array.Empty<string>();
            }

            LoadAllDrivers();
        }

        public List<ComboSelectListItem> GetAllDrivers()
        {
            var driverFilesComboSelect = new List<ComboSelectListItem>();
            using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
            var drivers = dc.Set<Driver>().AsNoTracking().ToList();

            foreach (var file in _driverFiles)
            {
                try
                {
                    var dll = Assembly.LoadFrom(file);
                    if (dll.GetTypes().Any(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass))
                    {
                        var fileName = Path.GetFileName(file);
                        var item = new ComboSelectListItem
                        {
                            Text = fileName,
                            Value = fileName,
                            Disabled = drivers.Any(x => x.FileName == fileName)
                        };
                        driverFilesComboSelect.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error loading assembly from {file}");
                }
            }

            return driverFilesComboSelect;
        }

        public string GetAssembleNameByFileName(string fileName)
        {
            var file = _driverFiles.SingleOrDefault(f => Path.GetFileName(f)
                                           .Equals(fileName, StringComparison.OrdinalIgnoreCase));
            if (file == null)
            {
                _logger.LogWarning($"File {fileName} not found in driver path.");
                return null;
            }

            try
            {
                var dll = Assembly.LoadFrom(file);
                var type = dll.GetTypes().FirstOrDefault(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass);
                return type?.FullName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading assembly for file {fileName}");
                return null;
            }
        }

        public void AddConfigs(Guid? deviceId, Guid? driverId)
        {
            using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
            var device = dc.Set<Device>().AsNoTracking().SingleOrDefault(x => x.ID == deviceId);
            var driver = dc.Set<Driver>().AsNoTracking().SingleOrDefault(x => x.ID == driverId);

            if (driver == null)
            {
                _logger.LogWarning($"Driver with ID {driverId} not found.");
                return;
            }

            var driverInfo = _driverInfos.SingleOrDefault(x => x.Type.FullName == driver.AssembleName);
            if (driverInfo == null)
            {
                _logger.LogWarning($"DriverInfo for assembly {driver.AssembleName} not found.");
                return;
            }

            // 获取带有参数(string, ILogger)的构造函数
            var constructor = driverInfo.Type.GetConstructor(new[] { typeof(string), typeof(ILogger) });
            if (constructor == null)
            {
                _logger.LogWarning($"No suitable constructor found for {driverInfo.Type.FullName}");
                return;
            }

            var instance = constructor.Invoke(new object[] { device?.DeviceName, _logger }) as IDriver;
            if (instance == null)
            {
                _logger.LogWarning($"Failed to create instance of {driverInfo.Type.FullName}");
                return;
            }

            foreach (var property in driverInfo.Type.GetProperties())
            {
                var configAttribute = property.GetCustomAttribute<ConfigParameterAttribute>();
                if (configAttribute != null)
                {
                    var value = property.GetValue(instance)?.ToString();
                    var deviceConfig = new DeviceConfig
                    {
                        ID = Guid.NewGuid(),
                        DeviceId = deviceId,
                        DeviceConfigName = property.Name,
                        DataSide = DataSide.AnySide,
                        Description = configAttribute.Description,
                        Value = value
                    };

                    if (property.PropertyType.IsEnum)
                    {
                        var enumValues = Enum.GetValues(property.PropertyType)
                                             .Cast<object>()
                                             .ToDictionary(val => Enum.GetName(property.PropertyType, val), val => (int)val);
                        deviceConfig.EnumInfo = JsonSerializer.Serialize(enumValues);
                    }

                    dc.Set<DeviceConfig>().Add(deviceConfig);
                }
            }

            dc.SaveChanges();
        }

        private void LoadAllDrivers()
        {
            _logger.LogInformation("LoadAllDrivers Start");
            foreach (var file in _driverFiles)
            {
                try
                {
                    var dll = Assembly.LoadFrom(file);
                    foreach (var type in dll.GetTypes().Where(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass))
                    {
                        _driverInfos.Add(new DriverInfo
                        {
                            FileName = Path.GetFileName(file),
                            Type = type
                        });
                        _logger.LogInformation($"LoadAllDrivers: {Path.GetFileName(file)} loaded.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"LoadAllDrivers Error for file {file}");
                }
            }
            _logger.LogInformation($"LoadAllDrivers End, Count: {_driverInfos.Count}");
        }
    }
}
