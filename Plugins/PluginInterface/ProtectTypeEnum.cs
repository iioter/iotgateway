using System.ComponentModel.DataAnnotations;

namespace PluginInterface
{
    public enum ProtectTypeEnum
    {
        [Display(Name = "ReadOnly")]
        ReadOnly,

        [Display(Name = "ReadWrite")]
        ReadWrite,

        [Display(Name = "WriteOnly")]
        WriteOnly,
    }
}