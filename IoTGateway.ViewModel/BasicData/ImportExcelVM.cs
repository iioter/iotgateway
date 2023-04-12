using Microsoft.EntityFrameworkCore;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using WalkingTec.Mvvm.Core.Support.FileHandlers;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using PluginInterface;
using IoTGateway.ViewModel.BasicData.DeviceVMs;
using Microsoft.Extensions.Logging;

namespace IoTGateway.ViewModel.BasicData
{
    public class ImportExcelVM : BaseVM
    {

        [Display(Name = "全部覆盖")]
        public bool Cover { get; set; }
        public string 导入结果 { get; set; }


        [Display(Name = "Excel模板文件")]
        public Guid ExcelFileId { get; set; }
        public FileAttachment ExcelFile { get; set; }

        private List<Driver> _drivers;
        private List<Device> _devices;
        private List<DeviceVariable> _deviceVariables;

        private Dictionary<string, DataTypeEnum> dateTypeNameMapping;
        private Dictionary<string, EndianEnum> endianTypeNameMapping;

        public void Import()
        {
            using var transaction = DC.BeginTransaction();
            _drivers = DC.Set<Driver>().AsNoTracking().ToList();
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            try
            {
                var oldDevices = DC.Set<Device>().ToList();

                if (true)//Cover
                {
                    foreach (var device in oldDevices)
                    {
                        var dapConfigs = DC.Set<DeviceConfig>().Where(x => x.DeviceId == device.ID).ToList();
                        var dapVariables = DC.Set<DeviceVariable>().Where(x => x.DeviceId == device.ID).ToList();
                        var rpcs = DC.Set<RpcLog>().Where(x => x.DeviceId == device.ID).ToList();
                        //var dapGroups = DC.Set<VariableGroup>().Where(x => x.DeviceId == dap.ID).ToList();
                        DC.Set<DeviceConfig>().RemoveRange(dapConfigs);
                        DC.Set<DeviceVariable>().RemoveRange(dapVariables);
                        DC.Set<RpcLog>().RemoveRange(rpcs);

                        deviceService.RemoveDeviceThread(device);
                    }
                    DC.Set<Device>().RemoveRange(oldDevices);
                    DC.SaveChanges();
                    DeleteDevices.doDelete(deviceService, DC, oldDevices.Select(x => x.ID).ToList());
                }
                var fp = Wtm.ServiceProvider.GetRequiredService<WtmFileProvider>();
                var file = fp.GetFile(ExcelFileId.ToString(), true, DC);
                var xssfworkbook = new XSSFWorkbook(file.DataStream);
                file.DataStream.Dispose();

                //获取数据的Sheet页信息
                var sheetDevice = xssfworkbook.GetSheet("设备维护");
                var devices = GetDevices(sheetDevice);
                DC.Set<Device>().AddRange(devices);

                var sheetVariables = xssfworkbook.GetSheet("变量配置");
                var deviceVariables = GetVariables(sheetVariables);
                DC.Set<DeviceVariable>().AddRange(deviceVariables);


                var sheetDeviceConfigs = xssfworkbook.GetSheet("通讯配置");
                var deviceConfigs = GetDeviceConfigs(sheetDeviceConfigs);
                DC.Set<DeviceConfig>().AddRange(deviceConfigs);


                var sheetSystemConfig = xssfworkbook.GetSheet("传输配置");
                var newSystemConfig = GetSystemConfig(sheetSystemConfig);
                var systemConfig = DC.Set<SystemConfig>().First();
                systemConfig.GatewayName = newSystemConfig.GatewayName;
                systemConfig.ClientId = newSystemConfig.ClientId;
                systemConfig.IoTPlatformType = newSystemConfig.IoTPlatformType;
                systemConfig.MqttIp = newSystemConfig.MqttIp;
                systemConfig.MqttPort = newSystemConfig.MqttPort;
                systemConfig.MqttUName = newSystemConfig.MqttUName;
                systemConfig.MqttUPwd = newSystemConfig.MqttUPwd;
                DC.SaveChanges();

                transaction.Commit();

                var myMqttClient = Wtm.ServiceProvider.GetService(typeof(MyMqttClient)) as MyMqttClient;
                myMqttClient.StartClientAsync().Wait();

                //重新启动采集
                foreach (var device in devices.Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device && x.DriverId != null))
                {
                    device.Driver = _drivers.FirstOrDefault(x => x.ID == device.DriverId);
                    deviceService.CreateDeviceThread(device);
                }
                导入结果 =
                    $"成功导入{devices.Count(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)}个设备，{devices.Count(x => x.DeviceTypeEnum == DeviceTypeEnum.Group)}个组";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                导入结果 = $"导入失败,已经回滚。{ex}";
                Console.WriteLine($"{导入结果},{ex.Message}");
            }
        }


        private List<Device> GetDevices(ISheet sheetDevice)
        {
            var devices = new List<Device>();
            for (int i = 1; i <= sheetDevice.LastRowNum; i++)
            {
                try
                {
                    var row = sheetDevice.GetRow(i);
                    Device device = new Device();
                    device.ID = Guid.NewGuid();
                    device.DeviceName = row.GetCell(0)?.ToString();
                    device.Index = uint.Parse(row.GetCell(1)?.ToString());
                    device.ParentId = devices.FirstOrDefault(x => x.DeviceName == row.GetCell(8)?.ToString() && x.DeviceTypeEnum == DeviceTypeEnum.Group)?.ID;
                    device.DeviceTypeEnum = device.ParentId == null ? DeviceTypeEnum.Group : DeviceTypeEnum.Device;

                    if (device.DeviceTypeEnum == DeviceTypeEnum.Device)
                    {
                        device.DriverId = _drivers.FirstOrDefault(x => x.DriverName == row.GetCell(2)?.ToString())?.ID;
                        device.AutoStart = row.GetCell(3).BooleanCellValue;
                        device.CgUpload = row.GetCell(4).BooleanCellValue;
                        device.EnforcePeriod = uint.Parse(row.GetCell(5)?.ToString());
                        device.CmdPeriod = uint.Parse(row.GetCell(6)?.ToString());
                    }
                    devices.Add(device);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            _devices = devices;
            return devices;
        }

        private List<DeviceVariable> GetVariables(ISheet sheet)
        {
            DateTypeNameMapping();
            EndianNameMapping();

            var deviceVariables = new List<DeviceVariable>();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                try
                {
                    var row = sheet.GetRow(i);
                    var variable = new DeviceVariable();
                    variable.ID = Guid.NewGuid();
                    variable.DeviceId = _devices.FirstOrDefault(x => x.DeviceName == row.GetCell(0)?.ToString())?.ID;
                    variable.Name = row.GetCell(1)?.ToString();
                    variable.Method = row.GetCell(2)?.ToString();
                    variable.DeviceAddress = row.GetCell(3)?.ToString();
                    variable.DataType = dateTypeNameMapping.ContainsKey(row.GetCell(4)?.ToString()) ? dateTypeNameMapping[row.GetCell(4)?.ToString()] : DataTypeEnum.Any;
                    variable.EndianType = endianTypeNameMapping.ContainsKey(row.GetCell(5)?.ToString()) ? endianTypeNameMapping[row.GetCell(5)?.ToString()] : EndianEnum.None;
                    variable.Expressions = row.GetCell(6)?.ToString();
                    variable.Alias = row.GetCell(7)?.ToString();
                    variable.IsUpload = row.GetCell(8)?.ToString().ToLower() == "false" ? false : true;
                    variable.Index = string.IsNullOrWhiteSpace(row.GetCell(9)?.ToString())
                        ? 999
                        : uint.Parse(row.GetCell(9).ToString());

                    deviceVariables.Add(variable);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            _deviceVariables = deviceVariables;
            return deviceVariables;
        }

        private List<DeviceConfig> GetDeviceConfigs(ISheet sheet)
        {
            var deviceConfigs = new List<DeviceConfig>();
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                try
                {
                    var row = sheet.GetRow(i);
                    var config = new DeviceConfig();
                    config.ID = Guid.NewGuid();
                    config.DeviceId = _devices.FirstOrDefault(x => x.DeviceName == row.GetCell(0)?.ToString())?.ID;
                    config.DeviceConfigName = row.GetCell(1)?.ToString();
                    config.DataSide = row.GetCell(2)?.ToString() == "共享属性" ? DataSide.AnySide : DataSide.ClientSide;
                    config.Description = row.GetCell(3)?.ToString();
                    config.Value = row.GetCell(4)?.ToString();
                    config.EnumInfo = row.GetCell(5)?.ToString();

                    deviceConfigs.Add(config);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            return deviceConfigs;
        }

        private SystemConfig GetSystemConfig(ISheet sheet)
        {
            var systemConfig = new SystemConfig();
            for (int i = 1; i <= 1; i++)
            {
                try
                {
                    var row = sheet.GetRow(i);
                    systemConfig.GatewayName = row.GetCell(0)?.ToString();
                    systemConfig.ClientId = row.GetCell(1)?.ToString();
                    systemConfig.IoTPlatformType = IoTPlatformType.IoTSharp;
                    systemConfig.MqttIp = row.GetCell(3)?.ToString();
                    systemConfig.MqttPort = int.Parse(row.GetCell(4)?.ToString());
                    systemConfig.MqttUName = row.GetCell(5)?.ToString();
                    systemConfig.MqttUPwd = row.GetCell(6)?.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            return systemConfig;
        }

        protected void DateTypeNameMapping()
        {
            dateTypeNameMapping = new Dictionary<string, DataTypeEnum>();

            var enumType = typeof(DataTypeEnum);
            var displayAttributeType = typeof(DisplayAttribute);
            DataTypeEnum? found = null;

            foreach (var name in Enum.GetNames(enumType))
            {
                var member = enumType.GetMember(name).First();
                var displayAttrib = (DisplayAttribute)member.GetCustomAttributes(displayAttributeType, false).First();
                dateTypeNameMapping.Add(displayAttrib.Name, (DataTypeEnum)Enum.Parse(enumType, name));
            }

        }

        protected void EndianNameMapping()
        {
            endianTypeNameMapping = new Dictionary<string, EndianEnum>();

            var enumType = typeof(EndianEnum);
            var displayAttributeType = typeof(DisplayAttribute);
            EndianEnum? found = null;

            Enum.GetNames(enumType).ToList().ForEach(name =>
            {
                var member = enumType.GetMember(name).First();
                var displayAttrib = (DisplayAttribute)member.GetCustomAttributes(displayAttributeType, false).First();
                endianTypeNameMapping.Add(displayAttrib.Name, (EndianEnum)Enum.Parse(enumType, name));
            });

        }

    }
}
