using PluginInterface;
using IoTClient.Clients.PLC;
using Microsoft.Extensions.Logging;

namespace PLC.OmronFins
{
    [DriverSupported("OmronFins")]
    [DriverInfo("OmronFins", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceOmronFins : IDriver
    {
        private OmronFinsClient? _plc;
        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 6000;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        #region 生命周期

        /// <summary>
        /// 反射构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="logger"></param>
        public DeviceOmronFins(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _plc is { Connected: true };

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");

                _plc = new OmronFinsClient(IpAddress, Port);
                _plc.Open();

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");
                return false;
            }
            return IsConnected;
        }


        /// <summary>
        /// 断开
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Close()");

                _plc?.Close();
                _plc = null;
                return !IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Close(),Error");
                return false;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Dispose()");

                // Suppress finalization.
                GC.SuppressFinalize(this);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Dispose(),Error");
            }
        }
        #endregion

        #region 读写方法

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("读PLC标准地址", description: "读PLC标准地址")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (_plc != null && this.IsConnected)
            {
                try
                {
                    switch (ioArg.ValueType)
                    {
                        case DataTypeEnum.Bit:
                            ret.Value = _plc.ReadBoolean(ioArg.Address).Value == true ? 1 : 0;
                            break;
                        case DataTypeEnum.Bool:
                            ret.Value = _plc.ReadBoolean(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.UByte:
                            ret.Value = _plc.ReadByte(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Byte:
                            ret.Value = (sbyte)_plc.ReadByte(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Uint16:
                            ret.Value = _plc.ReadUInt16(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Int16:
                            ret.Value = _plc.ReadInt16(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Uint32:
                            ret.Value = _plc.ReadUInt32(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Int32:
                            ret.Value = _plc.ReadInt32(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Float:
                            ret.Value = _plc.ReadFloat(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Double:
                            ret.Value = _plc.ReadDouble(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Uint64:
                            ret.Value = _plc.ReadUInt64(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.Int64:
                            ret.Value = _plc.ReadInt64(ioArg.Address).Value;
                            break;
                        case DataTypeEnum.AsciiString:
                            ret.Value = _plc.ReadString(ioArg.Address);
                            break;
                        case DataTypeEnum.Utf8String:
                            ret.Value = _plc.ReadString(ioArg.Address);
                            break;
                        default:
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败,不支持的类型:{ioArg.ValueType}";
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

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="method"></param>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioArg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            await Task.CompletedTask;
            return rpcResponse;
        }
        #endregion

    }
}