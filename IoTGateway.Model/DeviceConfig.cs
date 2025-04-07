using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("通讯配置")]
    [Index(nameof(DeviceConfigName))]
    [Index(nameof(Value))]
    public class DeviceConfig : BasePoco
    {
        [Comment("名称")]
        [Display(Name = "ConfigName")]
        public string DeviceConfigName { get; set; }

        [Comment("属性侧")]
        [Display(Name = "DataSide")]
        public DataSide DataSide { get; set; }

        [Comment("描述")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Comment("值")]
        [Display(Name = "Value")]
        public string Value { get; set; }

        [Comment("备注")]
        [Display(Name = "Remark")]
        public string EnumInfo { get; set; }

        public Device Device { get; set; }

        [Comment("所属设备")]
        [Display(Name = "Device")]
        public Guid? DeviceId { get; set; }
    }
}