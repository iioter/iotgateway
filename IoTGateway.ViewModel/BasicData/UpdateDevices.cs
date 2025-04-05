using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using IoTGateway.Model;

namespace IoTGateway.ViewModel.BasicData
{
    internal static class UpdateDevices
    {
        internal enum FromVM
        {
            Variable,
            Config,
            Device
        }

        public static void UpdateVariable(IDataContext DC, DeviceService deviceService, Dictionary<string, object> FC)
        {
            var devices = GetDevices(DC, FromVM.Variable, FC);
            deviceService.UpdateDevices(devices);
        }

        internal static void UpdateConfig(IDataContext DC, DeviceService deviceService, Dictionary<string, object> FC)
        {
            var devices = GetDevices(DC, FromVM.Config, FC);
            deviceService.UpdateDevices(devices);
        }

        internal static void UpdateDevice(IDataContext DC, DeviceService deviceService, Dictionary<string, object> FC)
        {
            var devices = GetDevices(DC, FromVM.Device, FC);
            deviceService.UpdateDevices(devices);
        }

        /// <summary>
        /// 获取包含相关导航属性的设备数据
        /// </summary>
        private static Device GetDeviceWithIncludes(IDataContext DC, Guid? deviceId)
        {
            return DC.Set<Device>()
                     .AsNoTracking()
                     .Include(x => x.Parent)
                     .Include(x => x.DeviceVariables)
                     .Include(x => x.Driver)
                     .Include(x => x.DeviceConfigs)
                     .SingleOrDefault(x => x.ID == deviceId);
        }

        internal static List<Device> GetDevices(IDataContext DC, FromVM fromVM, Dictionary<string, object> FC)
        {
            var devices = new List<Device>();
            var deviceIdsAdded = new HashSet<Guid>();

            // 如果 FC 包含 "Entity.DeviceId"，则优先添加该设备
            if (FC.TryGetValue("Entity.DeviceId", out var entityDeviceObj) && entityDeviceObj is StringValues entityDeviceValues)
            {
                if (entityDeviceValues.Count > 0 && Guid.TryParse(entityDeviceValues[0], out Guid entityDeviceId))
                {
                    var device = GetDeviceWithIncludes(DC, entityDeviceId);
                    if (device != null && deviceIdsAdded.Add(device.ID))
                    {
                        devices.Add(device);
                    }
                }
            }

            // 从 FC 中获取 Guid 列表
            var ids = FC2Guids(FC);
            switch (fromVM)
            {
                case FromVM.Variable:
                    foreach (var id in ids)
                    {
                        var deviceVariable = DC.Set<DeviceVariable>().SingleOrDefault(x => x.ID == id);
                        if (deviceVariable != null)
                        {
                            var device = GetDeviceWithIncludes(DC, deviceVariable.DeviceId);
                            if (device != null && deviceIdsAdded.Add(device.ID))
                            {
                                devices.Add(device);
                            }
                        }
                    }
                    break;
                case FromVM.Config:
                    foreach (var id in ids)
                    {
                        var deviceConfig = DC.Set<DeviceConfig>().AsNoTracking().SingleOrDefault(x => x.ID == id);
                        if (deviceConfig != null)
                        {
                            var device = GetDeviceWithIncludes(DC, deviceConfig.DeviceId);
                            if (device != null && deviceIdsAdded.Add(device.ID))
                            {
                                devices.Add(device);
                            }
                        }
                    }
                    break;
                case FromVM.Device:
                    foreach (var id in ids)
                    {
                        var device = GetDeviceWithIncludes(DC, id);
                        if (device != null && deviceIdsAdded.Add(device.ID))
                        {
                            devices.Add(device);
                        }
                    }
                    break;
                default:
                    break;
            }
            return devices;
        }

        internal static List<Guid> FC2Guids(Dictionary<string, object> FC)
        {
            var guids = new List<Guid>();
            StringValues idsStr = default;
            if (FC.TryGetValue("Ids", out var val) && val is StringValues sv1)
            {
                idsStr = sv1;
            }
            else if (FC.TryGetValue("Ids[]", out var val2) && val2 is StringValues sv2)
            {
                idsStr = sv2;
            }
            else if (FC.TryGetValue("id", out var val3) && val3 is StringValues sv3)
            {
                idsStr = sv3;
            }

            foreach (var item in idsStr)
            {
                if (Guid.TryParse(item, out Guid guid))
                {
                    guids.Add(guid);
                }
            }
            return guids;
        }
    }

}
