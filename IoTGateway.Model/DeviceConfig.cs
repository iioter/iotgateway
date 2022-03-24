using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class DeviceConfig : BasePoco
    {
        [Display(Name = "名称")]
        public string DeviceConfigName { get; set; }
        [Display(Name = "属性侧")]
        public DataSide DataSide { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        [Display(Name = "值")]
        public string Value { get; set; }
        [Display(Name = "备注")]
        public string EnumInfo { get; set; }
        public Device Device { get; set; }
        [Display(Name = "设备")]
        public Guid? DeviceId { get; set; }
    }
}
