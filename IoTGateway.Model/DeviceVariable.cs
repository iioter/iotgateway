using PluginInterface;
using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class DeviceVariable : TopBasePoco, IVariable
    {
        [Display(Name = "变量名")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "方法")]
        public string Method { get; set; }

        [Display(Name = "地址")]
        public string DeviceAddress { get; set; }

        [Display(Name = "类型")]
        public PluginInterface.DataTypeEnum DataType { get; set; }

        [Display(Name = "表达式")]
        public string Expressions { get; set; }

        [Display(Name = "权限")]
        public ProtectTypeEnum ProtectType { get; set; }

        public Device Device { get; set; }
        [Display(Name = "设备")]
        public Guid? DeviceId { get; set; }
    }
}