using System.ComponentModel.DataAnnotations;

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