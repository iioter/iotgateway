using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class DeviceConfig : BasePoco
    {
        [Display(Name = "Name")]
        public string DeviceConfigName { get; set; }
        [Display(Name = "attribute side")]
        public DataSide DataSide { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Value")]
        public string Value { get; set; }
        [Display(Name = "Remark")]
        public string EnumInfo { get; set; }
        public Device Device { get; set; }
        [Display(Name = "ID")]
        public Guid? DeviceId { get; set; }
    }
}
