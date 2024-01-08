using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class DeviceConfig : BasePoco
    {
        [Display(Name = "ConfigName")]
        public string DeviceConfigName { get; set; }
        [Display(Name = "DataSide")]
        public DataSide DataSide { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Value")]
        public string Value { get; set; }
        [Display(Name = "Remark")]
        public string EnumInfo { get; set; }
        public Device Device { get; set; }
        [Display(Name = "Device")]
        public Guid? DeviceId { get; set; }
    }
}
