using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
