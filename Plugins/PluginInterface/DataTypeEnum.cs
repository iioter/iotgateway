using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public enum DataTypeEnum
    {
        [Display(Name = "bit")]
        Bit,
        [Display(Name = "bool")]
        Bool,
        [Display(Name = "uint8")]
        UByte,
        [Display(Name = "int8")]
        Byte,
        [Display(Name = "uint16")]
        Uint16,
        [Display(Name = "int16")]
        Int16,
        [Display(Name = "bcd16")]
        Bcd16,
        [Display(Name = "uint32")]
        Uint32,
        [Display(Name = "int32")]
        Int32,
        [Display(Name = "float")]
        Float,
        [Display(Name = "bcd32")]
        Bcd32,
        [Display(Name = "uint64")]
        Uint64,
        [Display(Name = "int64")]
        Int64,
        [Display(Name = "double")]
        Double,
        [Display(Name = "ascii")]
        AsciiString,
        [Display(Name = "utf8")]
        Utf8String,
        [Display(Name = "datetime")]
        DateTime,
        [Display(Name = "timestamp(ms)")]
        TimeStampMs,
        [Display(Name = "timestamp(s)")]
        TimeStampS,
        [Display(Name = "任意类型")]
        Any,
        [Display(Name = "自定义1")]
        Custome1,
        [Display(Name = "自定义2")]
        Custome2,
        [Display(Name = "自定义3")]
        Custome3,
        [Display(Name = "自定义4")]
        Custome4,
        [Display(Name = "自定义5")]
        Custome5,
        ABCD,
    }
}
