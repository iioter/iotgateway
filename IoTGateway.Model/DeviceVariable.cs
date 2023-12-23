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
        [Display(Name = "Tag Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Method")]
        public string Method { get; set; }

        [Display(Name = "Address")]
        public string DeviceAddress { get; set; }

        [Display(Name = "DataType")]
        public DataTypeEnum DataType { get; set; }

        [Display(Name = "EndianType")]
        public EndianEnum EndianType { get; set; }

        [Display(Name = "Expressions")]
        public string Expressions { get; set; }

        [Display(Name = "Upload")]
        public bool IsUpload { get; set; }

        [Display(Name = "Permissions")]
        public ProtectTypeEnum ProtectType { get; set; }

        [Display(Name = "sort")]
        public uint Index { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Device Device { get; set; }
        [Display(Name = "ID")]
        public Guid? DeviceId { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [NotMapped]
        [Display(Name = "Value")]
        public object Value { get; set; }
        [NotMapped]
        [Display(Name = "CookedValue")]
        public object CookedValue { get; set; }
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        [Display(Name = "Update time")]
        public DateTime Timestamp { get; set; }
        [NotMapped]
        [Display(Name = "State")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.UnKnow;

        [NotMapped][Display(Name = "The Most Recent Values")] public object[] Values { get; set; } = new object[3];
        public void EnqueueVariable(object value)
        {
            Values[2] = Values[1];
            Values[1] = Values[0];
            Values[0] = value;
        }
    }
}