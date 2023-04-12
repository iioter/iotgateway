using PluginInterface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        public DataTypeEnum DataType { get; set; }

        [Display(Name = "大小端")]
        public EndianEnum EndianType { get; set; }

        [Display(Name = "表达式")]
        public string Expressions { get; set; }

        [Display(Name = "上传")]
        public bool IsUpload { get; set; }

        [Display(Name = "权限")]
        public ProtectTypeEnum ProtectType { get; set; }

        [Display(Name = "排序")]
        public uint Index { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Device Device { get; set; }
        [Display(Name = "设备")]
        public Guid? DeviceId { get; set; }

        [Display(Name = "别名")]
        public string Alias { get; set; }

        [NotMapped]
        [Display(Name = "原值")]
        public object Value { get; set; }
        [NotMapped]
        [Display(Name = "值")]
        public object CookedValue { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        [Display(Name = "更新时间")]
        public DateTime Timestamp { get; set; }
        [NotMapped]
        [Display(Name = "状态")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.UnKnow;
    }
}