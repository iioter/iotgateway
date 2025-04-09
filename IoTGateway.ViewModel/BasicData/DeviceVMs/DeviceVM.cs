using IoTGateway.Model;
using Microsoft.EntityFrameworkCore;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

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
            AllDrivers = DC.Set<Driver>().GetSelectListItems(Wtm, y => y.FileName);
            AllParents = DC.Set<Device>().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Group).GetSelectListItems(Wtm, y => y.DeviceName);
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
                    deviceService.DriverManager.AddConfigs(this.Entity.ID, this.Entity.DriverId);
                    var device = DC.Set<Device>().Where(x => x.ID == Entity.ID).Include(x => x.Parent).Include(x => x.Driver).Include(x => x.DeviceVariables).SingleOrDefault();
                    deviceService.CreateDeviceThread(device);

                    var messageService = Wtm.ServiceProvider.GetService(typeof(MessageService)) as MessageService;
                    messageService.DeviceAdded(device);
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
            var messageService = Wtm.ServiceProvider.GetService(typeof(MessageService)) as MessageService;
            messageService.DeviceDeleted(Entity);
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