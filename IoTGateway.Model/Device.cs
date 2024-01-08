using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class Device : TreePoco<Device>, IBasePoco
    {
        [Display(Name = "DeviceName")]
        public string DeviceName { get; set; }

        [Display(Name = "Sort")]
        public uint Index { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public Driver Driver { get; set; }
        [Display(Name = "Driver")]
        public Guid? DriverId { get; set; }

        [Display(Name = "AutoStart")]
        public bool AutoStart { get; set; }

        [Display(Name = "ChangeUpload")]
        public bool CgUpload { get; set; }

        [Display(Name = "EnforcePeriodms")]
        public uint EnforcePeriod { get; set; }

        [Display(Name = "CmdPeriodms")]
        public uint CmdPeriod { get; set; }

        [Display(Name = "Type")]
        public DeviceTypeEnum DeviceTypeEnum { get; set; }

        [Display(Name = "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }
        [Display(Name = "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        [Display(Name = "UpdateBy")]
        public string UpdateBy { get; set; }

        public List<DeviceConfig> DeviceConfigs { get; set; }
        public List<DeviceVariable> DeviceVariables { get; set; }
    }
}
