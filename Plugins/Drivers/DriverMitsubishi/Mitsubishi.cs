using IoTClient.Clients.PLC;
using IoTClient.Enums;
using PluginInterface;
using Microsoft.Extensions.Logging;

namespace DriverMitsubishi
{
    [DriverSupported("A_1E")]
    [DriverSupported("Qna_3E")]
    [DriverInfo("Mitsubishi", "V1.0.0", "Copyright IoTGateway-IoTClient© 2022-01-15")]
    public class Mitsubishi : IDriver
    {
        private MitsubishiClient? _plc;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("PLC类型")] public MitsubishiVersion CpuType { get; set; } = MitsubishiVersion.Qna_3E;

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 6000;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        public Mitsubishi(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }


        public bool IsConnected
        {
            get { return _plc != null && _plc.Connected; }
        }

        public bool Connect()
        {
            try
            {
                _plc = new MitsubishiClient(MitsubishiVersion.Qna_3E, IpAddress, Port);
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
                _plc?.Close();
                return !IsConnected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                _plc = null;
            }
            catch (Exception ex)
            {
            }
        }

        [Method("读西门子PLC标准地址", description: "读西门子PLC标准地址")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (_plc != null && this.IsConnected)
            {
                try
                {
                    switch (ioarg.ValueType)
                    {
                        case PluginInterface.DataTypeEnum.Bit:
                            ret.Value = _plc.ReadBoolean(ioarg.Address).Value ? 1 : 0;
                            break;
                        case PluginInterface.DataTypeEnum.Bool:
                            ret.Value = _plc.ReadBoolean(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.UByte:
                            ret.Value = _plc.ReadByte(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Byte:
                            ret.Value = (sbyte)_plc.ReadByte(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint16:
                            ret.Value = _plc.ReadUInt16(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int16:
                            ret.Value = _plc.ReadInt16(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint32:
                            ret.Value = _plc.ReadUInt32(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int32:
                            ret.Value = _plc.ReadInt32(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Float:
                            ret.Value = _plc.ReadFloat(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Double:
                            ret.Value = _plc.ReadDouble(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint64:
                            ret.Value = _plc.ReadUInt64(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int64:
                            ret.Value = _plc.ReadInt64(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.AsciiString:
                            ret.Value = _plc.ReadString(ioarg.Address);
                            break;
                        case PluginInterface.DataTypeEnum.Utf8String:
                            ret.Value = _plc.ReadString(ioarg.Address);
                            break;
                        default:
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败,不支持的类型:{ioarg.ValueType}";
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
            return rpcResponse;
        }
    }
}