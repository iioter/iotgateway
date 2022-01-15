using System;

namespace PluginInterface
{
    public interface IDriver:IDisposable
    {
        public Guid DeviceId { get; }
        public bool IsConnected { get; }
        public int Timeout { get; }
        public uint MinPeriod { get; }
        public bool Connect();
        public bool Close();

        public DriverReturnValueModel Read(DriverAddressIoArgModel Ioarg);
    }
}
