using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.Extensions.Logging;

namespace Plugin
{
    public class DriverService//: IDependency
    {
        private readonly ILogger<DriverService> _logger;
        string DriverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"drivers/net6.0");
        string[] driverFiles;
        public List<DriverInfo> DriverInfos = new List<DriverInfo>();
        public DriverService(IConfiguration ConfigRoot, ILogger<DriverService> logger)
        {
            _logger = logger;
            try
            {
                _logger.LogInformation("LoadDriverFiles Start");
                driverFiles = Directory.GetFiles(DriverPath).Where(x => Path.GetExtension(x) == ".dll").ToArray();
                _logger.LogInformation($"LoadDriverFiles End，Count{driverFiles.Count()}");
            }
            catch (Exception ex)
            {
                _logger.LogError("LoadDriverFiles Error", ex);
            }
            LoadAllDrivers();
        }
        public List<ComboSelectListItem> GetAllDrivers()
        {
            List<ComboSelectListItem> driverFilesComboSelect = new List<ComboSelectListItem>();
            using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
            {
                var Drivers = DC.Set<Driver>().AsNoTracking().ToList();

                foreach (var file in driverFiles)
                {
                    var dll = Assembly.LoadFrom(file);
                    if (dll.GetTypes().Where(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass).Any())
                    {
                        var fileName = Path.GetFileName(file);
                        var item = new ComboSelectListItem
                        {
                            Text = fileName,
                            Value = fileName,
                            Disabled = false,
                        };
                        if (Drivers.Where(x => x.FileName == Path.GetFileName(file)).Any())
                            item.Disabled = true;
                        driverFilesComboSelect.Add(item);
                    }
                }
            }
            return driverFilesComboSelect;
        }

        

        public string GetAssembleNameByFileName(string fileName)
        {
            var file = driverFiles.Where(f => Path.GetFileName(f) == fileName).SingleOrDefault();
            var dll = Assembly.LoadFrom(file);
            var type = dll.GetTypes().Where(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass).FirstOrDefault();
            return type.FullName;
        }
        public void AddConfigs(Guid? dapID, Guid? DriverId)
        {
            using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
            {
                var driver = DC.Set<Driver>().Where(x => x.ID == DriverId).AsNoTracking().SingleOrDefault();
                var type = DriverInfos.Where(x => x.Type.FullName == driver.AssembleName).SingleOrDefault();

                Type[] types = new Type[1] { typeof(Guid) };
                object[] param = new object[1] { Guid.Parse("88888888-8888-8888-8888-888888888888") };

                ConstructorInfo constructor = type.Type.GetConstructor(types);
                var iObj = constructor.Invoke(param) as IDriver;

                foreach (var property in type.Type.GetProperties())
                {
                    var config = property.GetCustomAttribute(typeof(ConfigParameterAttribute));
                    if (config != null)
                    {
                        var DapConfig = new DeviceConfig
                        {
                            ID = Guid.NewGuid(),
                            DeviceId = dapID,
                            DeviceConfigName = property.Name,
                            DataSide= DataSide.AnySide,
                            Description = ((ConfigParameterAttribute)config).Description,
                            Value = property.GetValue(iObj)?.ToString()
                        };

                        if (property.PropertyType.BaseType == typeof(Enum))
                        {
                            var fields = property.PropertyType.GetFields(BindingFlags.Static | BindingFlags.Public);
                            var EnumInfos = fields.ToDictionary(f => f.Name, f => (int)f.GetValue(null));
                            DapConfig.EnumInfo = JsonSerializer.Serialize(EnumInfos);
                        }

                        DC.Set<DeviceConfig>().Add(DapConfig);
                    }
                }
                DC.SaveChanges();
            }
        }
        public void LoadAllDrivers()
        {
            _logger.LogInformation("LoadAllDrivers Start");
            foreach (var file in driverFiles)
            {
                try
                {
                    var dll = Assembly.LoadFrom(file);
                    foreach (var type in dll.GetTypes().Where(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass))
                    {
                        DriverInfo driverInfo = new DriverInfo
                        {
                            FileName = Path.GetFileName(file),
                            Type = type
                        };
                        DriverInfos.Add(driverInfo);
                        _logger.LogInformation($"LoadAllDrivers {driverInfo.FileName} OK");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug($"LoadAllDrivers Error {ex}");
                }

            }
            _logger.LogInformation($"LoadAllDrivers End,Count{DriverInfos.Count}");

        }

        public void LoadRegestedDeviers()
        {
            using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
            {
                var Drivers = DC.Set<Driver>().AsNoTracking().ToList();

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"drivers/net5.0");
                var files = Directory.GetFiles(path).Where(x => Path.GetExtension(x) == ".dll").ToArray();
                foreach (var file in files)
                {
                    var dll = Assembly.LoadFrom(file);
                    foreach (var type in dll.GetTypes().Where(x => typeof(IDriver).IsAssignableFrom(x) && x.IsClass))
                    {
                        DriverInfo driverInfo = new DriverInfo
                        {
                            FileName = Path.GetFileName(file),
                            Type = type
                        };
                        DriverInfos.Add(driverInfo);
                    }
                }
            }
        }
    }
}
