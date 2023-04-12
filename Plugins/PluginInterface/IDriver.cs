using System;
using Microsoft.Extensions.Logging;

namespace PluginInterface
{
    public interface IDriver : IDisposable
    {
        public string DeviceId { get; }
        public bool IsConnected { get; }
        public int Timeout { get; }
        public uint MinPeriod { get; }


        public ILogger _logger { get; set; }

        public bool Connect();
        public bool Close();
        //标准数据读取
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg);
        //Rpc写入
        public Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel ioArg);
    }
}
