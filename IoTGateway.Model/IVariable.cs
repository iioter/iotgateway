using PluginInterface;
using System.ComponentModel.DataAnnotations;

namespace IoTGateway.Model
{
    public interface IVariable
    {
        [Display(Name = "变量名")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "地址")]
        public string DeviceAddress { get; set; }

        [Display(Name = "数据类型")]
        public PluginInterface.DataTypeEnum DataType { get; set; }

        [Display(Name = "表达式")]
        public string Expressions { get; set; }

        [Display(Name = "权限")]
        public ProtectTypeEnum ProtectType { get; set; }
    }
}