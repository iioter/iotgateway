using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public class RpcResponse
    {
        public RpcResponse()
        {
            this.IsSuccess = false;
        }
        public string RequestId { get; set; }
        public string DeviceName { get; set; }
        public bool IsSuccess { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return $"RequestId:{RequestId},DeviceName:{DeviceName},Method:{Method},IsSuccess:{IsSuccess},Description:{Description}";
        }
    }
}
