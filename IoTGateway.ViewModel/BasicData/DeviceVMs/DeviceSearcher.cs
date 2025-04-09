using IoTGateway.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceSearcher : BaseSearcher
    {
        [Display(Name = "名称")]
        public String DeviceName { get; set; }

        public List<ComboSelectListItem> AllDrivers { get; set; }

        [Display(Name = "驱动")]
        public Guid? DriverId { get; set; }

        [Display(Name = "自启动")]
        public Boolean? AutoStart { get; set; }

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