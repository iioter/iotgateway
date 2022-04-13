using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.ThingsBoard
{
    //{"device": "Device A", "id": $request_id, "data": {"success": true}}
    public class TBRpcResponse
    {
        [JsonProperty(PropertyName = "device")]
        public string DeviceName { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string RequestId { get; set; }
        [JsonProperty(PropertyName = "data")]
        public Dictionary<string, object> ResponseData { get; set; }
    }
}
