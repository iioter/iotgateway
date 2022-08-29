using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Plugin;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceVM : BaseCRUDVM<Device>
    {
        public List<ComboSelectListItem> AllDrivers { get; set; }
        public List<ComboSelectListItem> AllParents { get; set; }

        public DeviceVM()
        {
            SetInclude(x => x.Driver);
            SetInclude(x => x.Parent);
        }

        protected override void InitVM()
        {
            AllDrivers = DC.Set<Driver>().GetSelectListItems(Wtm, y => y.DriverName);
            AllParents = DC.Set<Device>().Where(x=>x.DeviceTypeEnum== DeviceTypeEnum.Group).GetSelectListItems(Wtm, y => y.DeviceName);
        }

        public override void DoAdd()
        {
            try
            {
                base.DoAdd();
                //添加结束
                if (this.Entity.DeviceTypeEnum == DeviceTypeEnum.Device)
                {
                    var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                    deviceService.DrvierManager.AddConfigs(this.Entity.ID, this.Entity.DriverId);
                    var device = DC.Set<Device>().Where(x => x.ID == Entity.ID).Include(x=>x.Parent).Include(x => x.Driver).SingleOrDefault();
                    deviceService.CreateDeviceThread(device);

                    var myMqttClient = Wtm.ServiceProvider.GetService(typeof(MyMqttClient)) as MyMqttClient;
                    myMqttClient.DeviceAdded(device);
                }
            }
            catch (Exception ex)
            {
                MSD.AddModelError("", $"添加失败,{ex.Message}");
            }
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
            //修改结束
            var pluginManager = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            UpdateDevices.UpdateDevice(DC, pluginManager, FC);
        }

        public override void DoDelete()
        {
            List<Guid> Ids = new List<Guid>() { Guid.Parse(FC["id"].ToString()) };

            var pluginManager = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            var myMqttClient = Wtm.ServiceProvider.GetService(typeof(MyMqttClient)) as MyMqttClient;
            myMqttClient.DeviceDeleted(Entity);
            var ret = DeleteDevices.doDelete(pluginManager, DC, Ids);
            if (!ret.IsSuccess)
            {
                MSD.AddModelError("", ret.Message);
                return;
            }

        }
        public override DuplicatedInfo<Device> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(x => x.DeviceName));
            return rv;
        }
    }
}
