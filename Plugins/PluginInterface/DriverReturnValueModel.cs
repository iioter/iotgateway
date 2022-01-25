using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public class DriverReturnValueModel
    {
        public object Value { get; set; }
        public object CookedValue { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.UnKnow;
        public Guid VarId { get; set; }
    }
}
