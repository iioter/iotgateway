using PluginInterface;
using OpenNETCF.MTConnect;
using Microsoft.Extensions.Logging;

namespace CNC.MTConnect
{
    [DriverSupported("MTConnectClient")]
    [DriverInfo("MTConnectClient", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceMTClient : IDriver
    {
        private EntityClient? _mClient;
        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("uri")] public string Uri { get; set; }

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        public bool IsConnected { get; set; }

        #endregion

        #region 生命周期


        public DeviceMTClient(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        public bool Close()
        {
            try
            {
                _mClient = null;
                IsConnected = false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Connect()
        {
            try
            {
                _mClient = new EntityClient(Uri);
                _mClient.RequestTimeout = Timeout;
                IsConnected = true;
            }
            catch (Exception)
            {
                IsConnected = false;
            }

            return IsConnected;
        }

        public void Dispose()
        {

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 读写方法


        [Method("读MTConnect", description: "读MTConnect ID")]
        public DriverReturnValueModel ReadById(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    var dataValue = _mClient?.GetDataItemById(ioarg.Address).Value;
                    ret.Value = dataValue;
                }
                catch (Exception ex)
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = $"读取失败,{ex.Message}";
                }
            }
            else
            {
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                ret.Message = "连接失败";
            }

            return ret;
        }

        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            return ret;
        }

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return rpcResponse;
        }

        #endregion
    }
}