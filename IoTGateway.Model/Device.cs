using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class Device : TreePoco<Device>, IBasePoco
    {
        [Display(Name = "名称")]
        public string DeviceName { get; set; }

        [Display(Name = "排序")]
        public uint Index { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        public Driver Driver { get; set; }
        [Display(Name = "驱动")]
        public Guid? DriverId { get; set; }

        [Display(Name = "启动")]
        public bool AutoStart { get; set; }

        [Display(Name = "变化上传")]
        public bool CgUpload { get; set; }

        [Display(Name = "归档周期ms")]
        public uint EnforcePeriod { get; set; }

        [Display(Name = "指令间隔ms")]
        public uint CmdPeriod { get; set; }

        [Display(Name = "类型")]
        public DeviceTypeEnum DeviceTypeEnum { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
        [Display(Name = "创建人")]
        public string CreateBy { get; set; }
        [Display(Name = "更新时间")]
        public DateTime? UpdateTime { get; set; }
        [Display(Name = "更新人")]
        public string UpdateBy { get; set; }

        public List<DeviceConfig> DeviceConfigs { get; set; }
        public List<DeviceVariable> DeviceVariables { get; set; }
    }
}
