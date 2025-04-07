using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PluginInterface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("变量配置")]
    [Index(nameof(Name))]
    [Index(nameof(Method))]
    [Index(nameof(DeviceAddress))]
    [Index(nameof(DataType))]
    public class DeviceVariable : TopBasePoco, IVariable
    {
        [Comment("变量名")]
        [Display(Name = "VariableName")]
        public string Name { get; set; }

        [Comment("描述")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Comment("方法")]
        [Display(Name = "Method")]
        public string Method { get; set; }

        [Comment("地址")]
        [Display(Name = "Address")]
        public string DeviceAddress { get; set; }

        [Comment("数据类型")]
        [Display(Name = "DataType")]
        public DataTypeEnum DataType { get; set; }

        [Comment("触发")]
        [Display(Name = "IsTrigger")]
        public bool IsTrigger { get; set; }

        [Comment("大小端")]
        [Display(Name = "EndianType")]
        public EndianEnum EndianType { get; set; }

        [Comment("表达式")]
        [Display(Name = "Expressions")]
        public string Expressions { get; set; }

        [Comment("上传")]
        [Display(Name = "Upload")]
        public bool IsUpload { get; set; }

        [Comment("权限")]
        [Display(Name = "Permissions")]
        public ProtectTypeEnum ProtectType { get; set; }

        [Comment("排序")]
        [Display(Name = "Sort")]
        public uint Index { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Device Device { get; set; }

        [Comment("所属设备")]
        [Display(Name = "Device")]
        public Guid? DeviceId { get; set; }

        [Comment("别名")]
        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Comment("原值")]
        [NotMapped]
        [Display(Name = "RawValue")]
        public object Value { get; set; }

        [Comment("计算后的值")]
        [NotMapped]
        [Display(Name = "CookedValue")]
        public object CookedValue { get; set; }

        [Comment("错误信息")]
        [NotMapped]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Comment("更新时间")]
        [NotMapped]
        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }

        [Comment("状态")]
        [NotMapped]
        [Display(Name = "Status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.UnKnow;

        [Comment("最新三次原值")]
        [NotMapped][Display(Name = "MostRecentValues")] public object[] Values { get; set; } = new object[3];

        [Comment("最新三次计算后的值")]
        [NotMapped][Display(Name = "MostRecentCookedValues")] public object[] CookedValues { get; set; } = new object[3];

        public void EnqueueVariable(object value)
        {
            Values[2] = Values[1];
            Values[1] = Values[0];
            Values[0] = value;
        }

        public void EnqueueCookedVariable(object value)
        {
            CookedValues[2] = CookedValues[1];
            CookedValues[1] = CookedValues[0];
            CookedValues[0] = value;
        }
    }
}