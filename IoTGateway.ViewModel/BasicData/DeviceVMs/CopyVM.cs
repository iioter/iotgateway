using Microsoft.EntityFrameworkCore;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public class CopyVM : BaseVM
    {
        public string 设备名称 { get; set; }
        public uint 复制数量 { get; set; } = 1;
        public string 复制结果 { get; set; }

        public void Copy()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var device = DC.Set<Device>().Where(x => x.ID == Guid.Parse(FC["id"].ToString())).Include(x => x.Parent).Include(x => x.Driver).FirstOrDefault();
                    var devices = new List<Device>();
                    if (device == null)
                        复制结果 = "复制失败，找不到采集点";
                    else if (device.DeviceTypeEnum == DeviceTypeEnum.Group)
                        复制结果 = "复制失败，组不支持复制";
                    else
                    {
                        var deviceConfigs = DC.Set<DeviceConfig>().Where(x => x.DeviceId == device.ID).ToList();
                        var deviceVariables = DC.Set<DeviceVariable>().Where(x => x.DeviceId == device.ID).ToList();
                        for (int i = 1; i <= 复制数量; i++)
                        {
                            var newDevice = new Device
                            {
                                ID = Guid.NewGuid(),
                                DeviceName = $"{device.DeviceName}-Copy{i}",
                                AutoStart = false,
                                ParentId = device.ParentId,
                                CreateBy = this.Wtm.LoginUserInfo.Name,
                                CreateTime = DateTime.Now,
                                Driver = device.Driver,
                                DriverId = device.DriverId,
                                Description = device.Description,
                                DeviceTypeEnum = device.DeviceTypeEnum,
                                Parent= device.Parent
                            };
                            DC.Set<Device>().Add(newDevice);
                            devices.Add(newDevice);

                            foreach (var deviceConfig in deviceConfigs)
                            {
                                var newDeviceConfig = new DeviceConfig
                                {
                                    DeviceId = newDevice.ID,
                                    DeviceConfigName = deviceConfig.DeviceConfigName,
                                    Description = deviceConfig.Description,
                                    EnumInfo = deviceConfig.EnumInfo,
                                    Value = deviceConfig.Value,
                                    UpdateBy = this.Wtm.LoginUserInfo.Name,
                                    UpdateTime = DateTime.Now
                                };
                                DC.Set<DeviceConfig>().Add(newDeviceConfig);
                            }

                            foreach (var deviceVariable in deviceVariables)
                            {
                                var newDeviceVariable = new DeviceVariable
                                {
                                    DeviceId = newDevice.ID,
                                    Name = deviceVariable.Name,
                                    Description = deviceVariable.Description,
                                    DataType = deviceVariable.DataType,
                                    Method = deviceVariable.Method,
                                    ProtectType = deviceVariable.ProtectType,
                                    Expressions = deviceVariable.Expressions,
                                    DeviceAddress = deviceVariable.DeviceAddress
                                };
                                DC.Set<DeviceVariable>().Add(newDeviceVariable);

                            }
                        }
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    复制结果 = "复制成功";

                    var pluginManager = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                    pluginManager?.CreateDeviceThreads(devices);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    复制结果 = $"复制失败,{ex}";
                }
            }
        }

        protected override void InitVM()
        {
            var device = DC.Set<Device>().AsNoTracking().Include(x => x.Parent).Where(x => x.ID == Guid.Parse(FC["id"].ToString())).FirstOrDefault();
            设备名称 = $"{device?.Parent?.DeviceName}===>{device?.DeviceName}";

            base.InitVM();
        }
    }
}
