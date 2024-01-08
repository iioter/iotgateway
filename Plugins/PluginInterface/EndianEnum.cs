using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PluginInterface
{
    public enum EndianEnum
    {
        [Display(Name = "None")] None = 0,
        [Display(Name = "BigEndian")] BigEndian,
        [Display(Name = "LittleEndian")] LittleEndian,
        [Display(Name = "BigEndianSwap")] BigEndianSwap,
        [Display(Name = "LittleEndianSwap")] LittleEndianSwap
    }
}
