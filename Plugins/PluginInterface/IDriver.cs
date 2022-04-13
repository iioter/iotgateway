using System;

namespace PluginInterface
{
    public interface IDriver : IDisposable
    {
        public Guid DeviceId { get; }
        public bool IsConnected { get; }
        public int Timeout { get; }
        public uint MinPeriod { get; }
        public bool Connect();
        public bool Close();
        //标准数据读取
        public DriverReturnValueModel Read(DriverAddressIoArgModel Ioarg);
        //Rpc写入
        public Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel Ioarg);
    }
}
