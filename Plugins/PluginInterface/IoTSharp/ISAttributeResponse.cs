using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.IoTSharp
{
    public class ISAttributeResponse
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string KeyName { get; set; }

        public string Data { get; set; }
    }
}
