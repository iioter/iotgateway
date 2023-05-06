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
        [Display(Name = "只读")]
        ReadOnly,
        [Display(Name = "读写")]
        ReadWrite,
        [Display(Name = "只写")]
        WriteOnly,
    }
}
