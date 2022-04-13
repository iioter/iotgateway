using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.IoTSharp
{
    public class ISRpcResponse
    {
        public string DeviceId { get; set; }
        public string Method { get; set; }
        public string ResponseId { get; set; }
        public string Data { get; set; }
    }
}
