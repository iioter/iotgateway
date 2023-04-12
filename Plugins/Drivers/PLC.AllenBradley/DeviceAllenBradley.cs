using PluginInterface;
using IoTClient.Clients.PLC;
using Microsoft.Extensions.Logging;

namespace PLC.AllenBradley
{
    [DriverSupported("AllenBradley")]
    [DriverInfo("AllenBradley", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceAllenBradley : IDriver
    {
        private AllenBradleyClient? _plc;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 44818;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        #region 生命周期
        public DeviceAllenBradley(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{device}],Create()");
        }

        public bool IsConnected => _plc is { Connected: true };

        public bool Connect()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");
                _plc = new AllenBradleyClient(IpAddress, Port);
                _plc.Open();
            }
            catch (Exception)
            {
                return false;
            }

            return IsConnected;
        }

        public bool Close()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Close()");
                _plc?.Close();
                return !IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Device:[{_device}],Close()");
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                _plc = null;

                // Suppress finalization.
                GC.SuppressFinalize(this);

                _logger.LogInformation($"Device:[{_device}],Dispose()");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Dispose(),Error");
            }
        }
        #endregion

        #region 读写方法

        [Method("读AllenBradleyPLC标准地址", description: "读AllenBradleyPLC标准地址")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (_plc != null && this.IsConnected)
            {
                try
                {
                    switch (ioarg.ValueType)
                    {
                        case DataTypeEnum.Bit:
                            ret.Value = _plc.ReadBoolean(ioarg.Address).Value ? 1 : 0;
                            break;
                        case DataTypeEnum.Bool:
                            ret.Value = _plc.ReadBoolean(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.UByte:
                            ret.Value = _plc.ReadByte(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Byte:
                            ret.Value = (sbyte)_plc.ReadByte(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Uint16:
                            ret.Value = _plc.ReadUInt16(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Int16:
                            ret.Value = _plc.ReadInt16(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Uint32:
                            ret.Value = _plc.ReadUInt32(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Int32:
                            ret.Value = _plc.ReadInt32(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Float:
                            ret.Value = _plc.ReadFloat(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Double:
                            ret.Value = _plc.ReadDouble(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Uint64:
                            ret.Value = _plc.ReadUInt64(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.Int64:
                            ret.Value = _plc.ReadInt64(ioarg.Address).Value;
                            break;
                        case DataTypeEnum.AsciiString:
                            ret.Value = _plc.ReadString(ioarg.Address);
                            break;
                        case DataTypeEnum.Utf8String:
                            ret.Value = _plc.ReadString(ioarg.Address);
                            break;
                        default:
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"不支持的类型";
                            break;
                    }
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

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            await Task.CompletedTask;
            return rpcResponse;
        }

        #endregion

    }
}