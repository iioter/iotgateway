using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class Device : TreePoco<Device>, IBasePoco
    {
        [Display(Name = "Name")]
        public string DeviceName { get; set; }

        [Display(Name = "sort")]
        public uint Index { get; set; }

        [Display(Name = "describe")]
        public string Description { get; set; }

        public Driver Driver { get; set; }
        [Display(Name = "drive")]
        public Guid? DriverId { get; set; }

        [Display(Name = "start up")]
        public bool AutoStart { get; set; }

        [Display(Name = "Change upload")]
        public bool CgUpload { get; set; }

        [Display(Name = "Archiving cycle ms")]
        public uint EnforcePeriod { get; set; }

        [Display(Name = "Command interval ms")]
        public uint CmdPeriod { get; set; }

        [Display(Name = "Type")]
        public DeviceTypeEnum DeviceTypeEnum { get; set; }

        [Display(Name = "Creation time")]
        public DateTime? CreateTime { get; set; }
        [Display(Name = "Founder")]
        public string CreateBy { get; set; }
        [Display(Name = "Update time")]
        public DateTime? UpdateTime { get; set; }
        [Display(Name = "updater")]
        public string UpdateBy { get; set; }

        public List<DeviceConfig> DeviceConfigs { get; set; }
        public List<DeviceVariable> DeviceVariables { get; set; }
    }
}
