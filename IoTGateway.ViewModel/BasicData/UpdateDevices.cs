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
        internal static void UpdateVaribale(IDataContext DC, DeviceService deviceService, Dictionary<String, Object> FC)
        {
            var devices = GetDevices(DC, deviceService, FromVM.Variable, FC);
            deviceService.UpdateDevices(devices);
        }

        internal static void UpdateConfig(IDataContext DC, DeviceService deviceService, Dictionary<String, Object> FC)
        {
            var devices = GetDevices(DC,deviceService, FromVM.Config,FC); 
            deviceService.UpdateDevices(devices);
        }

        internal static void UpdateDevice(IDataContext DC, DeviceService deviceService, Dictionary<String, Object> FC)
        {
            var devices = GetDevices(DC, deviceService, FromVM.Device, FC);
            deviceService.UpdateDevices(devices);
        }
        

        internal static List<Device> GetDevices(IDataContext DC, DeviceService deviceService, FromVM fromVM, Dictionary<String, Object> FC)
        {
            List<Device> devices = new();
            List<Guid> Ids = FC2Guids(FC);

            if (FC.ContainsKey("Entity.DeviceId"))
            {
                StringValues id = (StringValues)FC["Entity.DeviceId"];
                var device = DC.Set<Device>().AsNoTracking().Include(x => x.Parent).Where(x => x.ID == Guid.Parse(id)).Include(x => x.DeviceVariables).Include(x => x.Driver).Include(x => x.DeviceConfigs).SingleOrDefault();
                if (!devices.Where(x => x.ID == device.ID).Any())
                    devices.Add(device);
            }
            foreach (var varId in Ids)
            {
                switch (fromVM)
                {
                    case FromVM.Variable:
                        var deviceVariable = DC.Set<DeviceVariable>().Where(x => x.ID == varId).SingleOrDefault();
                        if (deviceVariable != null)
                        {
                            var device = DC.Set<Device>().AsNoTracking().Include(x => x.Parent).Where(x => x.ID == deviceVariable.DeviceId).Include(x=>x.DeviceVariables).Include(x => x.Driver).Include(x => x.DeviceConfigs).SingleOrDefault();
                            if (!devices.Where(x => x.ID == device.ID).Any())
                                devices.Add(device);
                        }
                        break;
                    case FromVM.Config:
                        foreach (var deviceConfigId in Ids)
                        {
                            var deviceConfig = DC.Set<DeviceConfig>().AsNoTracking().Where(x => x.ID == deviceConfigId).SingleOrDefault();
                            if (deviceConfig != null)
                            {
                                var device = DC.Set<Device>().AsNoTracking().Where(x => x.ID == deviceConfig.DeviceId).Include(x => x.Parent).Include(x => x.DeviceVariables).Include(x => x.Driver).Include(x => x.DeviceConfigs).SingleOrDefault();
                                if (!devices.Where(x => x.ID == device.ID).Any())
                                    devices.Add(device);
                            }
                        }
                        break;
                    case FromVM.Device:
                        foreach (var deviceId in Ids)
                        {
                            var device = DC.Set<Device>().AsNoTracking().Include(x => x.Parent).Where(x => x.ID == deviceId).Include(x => x.DeviceVariables).Include(x => x.Driver).Include(x=>x.DeviceConfigs).SingleOrDefault();
                            if (!devices.Where(x => x.ID == device.ID).Any())
                                devices.Add(device);
                        }
                        break;
                    default:
                        break;
                }
                
            }
            return devices;
        }

        internal static List<Guid> FC2Guids(Dictionary<String, Object> FC)
        {
            List<Guid> Ids = new();
            StringValues IdsStr = new();
            if (FC.ContainsKey("Ids"))
                IdsStr = (StringValues)FC["Ids"];
            else if (FC.ContainsKey("Ids[]"))
                IdsStr = (StringValues)FC["Ids[]"];
            else if (FC.ContainsKey("id"))
                IdsStr = (StringValues)FC["id"];
            foreach (var item in IdsStr)
            {
                Ids.Add(Guid.Parse(item));
            }
            return Ids;
        }
    }
}
