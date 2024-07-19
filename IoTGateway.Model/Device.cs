using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("设备维护")]
    [Index(nameof(DeviceName))]
    [Index(nameof(AutoStart))]
    [Index(nameof(DeviceTypeEnum))]
    public class Device : TreePoco<Device>, IBasePoco
    {
        [Comment("名称")]
        [Display(Name = "DeviceName")]
        public string DeviceName { get; set; }

        [Comment("排序")]
        [Display(Name = "Sort")]
        public uint Index { get; set; }

        [Comment("描述")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public Driver Driver { get; set; }
        [Comment("驱动")]
        [Display(Name = "Driver")]
        public Guid? DriverId { get; set; }

        [Comment("启动")]
        [Display(Name = "AutoStart")]
        public bool AutoStart { get; set; }

        [Comment("变化上传")]
        [Display(Name = "ChangeUpload")]
        public bool CgUpload { get; set; }

        [Comment("归档周期ms")]
        [Display(Name = "EnforcePeriodms")]
        public uint EnforcePeriod { get; set; }

        [Comment("指令间隔ms")]
        [Display(Name = "CmdPeriodms")]
        public uint CmdPeriod { get; set; }

        [Comment("类型(组或设备)")]
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
