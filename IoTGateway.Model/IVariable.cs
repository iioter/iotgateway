using PluginInterface;
using System.ComponentModel.DataAnnotations;

namespace IoTGateway.Model
{
    public interface IVariable
    {
        [Display(Name = "Variable Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Address")]
        public string DeviceAddress { get; set; }

        [Display(Name = "DataType")]
        public PluginInterface.DataTypeEnum DataType { get; set; }

        [Display(Name = "Expressions")]
        public string Expressions { get; set; }

        [Display(Name = "Permissions")]
        public ProtectTypeEnum ProtectType { get; set; }
    }
}