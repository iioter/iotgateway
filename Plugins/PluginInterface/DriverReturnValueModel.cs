using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public class DriverReturnValueModel
    {
        public DriverReturnValueModel()
        {
            this.Timestamp = DateTime.Now;
        }

        public DriverReturnValueModel(VaribaleStatusTypeEnum status)
        {
            this.Timestamp = DateTime.Now; ;
            StatusType = status;
        }
        public DriverReturnValueModel(VaribaleStatusTypeEnum status,string message)
        {
            this.Timestamp = DateTime.Now;
            StatusType= status;
            this.Message = message;
        }

        public object Value { get; set; }
        public object CookedValue { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.UnKnow;
        public Guid VarId { get; set; }
    }
}
