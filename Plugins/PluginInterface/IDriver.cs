using Microsoft.Extensions.Logging;

namespace PluginInterface
{
    // 数据上报事件参数
    public class DataReportEventArgs : EventArgs
    {
        public string DeviceId { get; set; } = string.Empty;
        public string VariableName { get; set; } = string.Empty;
        public object? Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public VaribaleStatusTypeEnum StatusType { get; set; } = VaribaleStatusTypeEnum.Good;
        public string Message { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public interface IDriver : IDisposable
    {
        public string DeviceId { get; }
        public bool IsConnected { get; }
        public int Timeout { get; }
        public uint MinPeriod { get; }

        public ILogger _logger { get; set; }

        // 异步数据上报事件
        public event Func<object, DataReportEventArgs, Task>? OnDataReceived;

        public bool Connect();

        public bool Close();

        //标准数据读取
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg);

        //Rpc写入
        public Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel ioArg);
    }
}
