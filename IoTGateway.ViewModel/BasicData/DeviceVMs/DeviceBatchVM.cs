using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Microsoft.Extensions.Primitives;
using Plugin;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceBatchVM : BaseBatchVM<Device, Device_BatchEdit>
    {
        public DeviceBatchVM()
        {
            ListVM = new DeviceListVM();
            LinkedVM = new Device_BatchEdit();
        }
        public override bool DoBatchDelete()
        {
            StringValues IdsStr = new();
            if (FC.ContainsKey("Ids"))
                IdsStr = (StringValues)FC["Ids"];
            else if (FC.ContainsKey("Ids[]"))
                IdsStr = (StringValues)FC["Ids[]"];
            List<Guid> Ids = new();
            foreach (var item in IdsStr)
            {
                Ids.Add(Guid.Parse(item));
            }

            var pluginManager = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            var ret = DeleteDevices.doDelete(pluginManager, DC, Ids);
            if (!ret.IsSuccess)
            {
                MSD.AddModelError("", ret.Message);
                return false;
            }

            return true;
        }
        protected override void InitVM()
        {
            base.InitVM();
        }

        public override bool DoBatchEdit()
        {
            var ret = base.DoBatchEdit();
            if (ret)
            {
                var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                UpdateDevices.UpdateDevice(DC, deviceService, FC);
            }
            return ret;
        }
    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class Device_BatchEdit : BaseVM
    {
        public List<ComboSelectListItem> AllDrivers { get; set; }
        public Guid? DriverId { get; set; }
        [Display(Name = "自启动")]
        public Boolean? AutoStart { get; set; }
        [Display(Name = "变化上传")]
        public Boolean CgUpload { get; set; }
        [Display(Name = "归档周期ms")]
        public uint EnforcePeriod { get; set; }
        [Display(Name = "类型")]
        public DeviceTypeEnum? DeviceTypeEnum { get; set; }
        public List<ComboSelectListItem> AllParents { get; set; }
        [Display(Name = "_Admin.Parent")]
        public Guid? ParentId { get; set; }

        protected override void InitVM()
        {
            AllDrivers = DC.Set<Driver>().GetSelectListItems(Wtm, y => y.DriverName);
            AllParents = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }

}
