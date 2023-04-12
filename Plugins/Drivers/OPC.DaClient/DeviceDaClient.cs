using PluginInterface;
using Automation.OPCClient;
using Microsoft.Extensions.Logging;

namespace OPC.DaClient
{
    [DriverSupported("OPCDaClient")]
    [DriverInfo("OPCDaClient", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    internal class DeviceDaClient : IDriver
    {
        private OPCClientWrapper? _opcDaClient;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP")] public string Ip { get; set; } = "127.0.0.1";

        [ConfigParameter("OpcServerName")] public string OpcServerName { get; set; } = "ICONICS.SimulatorOPCDA.2";

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        #region 生命周期

        /// <summary>
        /// 反射构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="logger"></param>
        public DeviceDaClient(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _opcDaClient != null && _opcDaClient.IsOPCServerConnected();

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");

                _opcDaClient = new OPCClientWrapper();
                _opcDaClient.Init(Ip, OpcServerName);
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

                _opcDaClient?.Disconnect();
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

                _opcDaClient = null;

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


        [Method("读OPCDa", description: "读OPCDa节点")]
        public DriverReturnValueModel ReadNode(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    var dataValue = _opcDaClient?.ReadNodeLabel(ioArg.Address);
                    switch (ioArg.ValueType)
                    {
                        case DataTypeEnum.Bit:
                            ret.Value = dataValue == "On" ? 1 : 0;
                            break;
                        case DataTypeEnum.Bool:
                            ret.Value = dataValue == "On";
                            break;
                        case DataTypeEnum.Byte:
                            if (dataValue != null) ret.Value = byte.Parse(dataValue);
                            break;
                        case DataTypeEnum.UByte:
                            if (dataValue != null) ret.Value = sbyte.Parse(dataValue);
                            break;
                        case DataTypeEnum.Int16:
                            if (dataValue != null) ret.Value = short.Parse(dataValue);
                            break;
                        case DataTypeEnum.Uint16:
                            if (dataValue != null) ret.Value = ushort.Parse(dataValue);
                            break;
                        case DataTypeEnum.Int32:
                            if (dataValue != null) ret.Value = int.Parse(dataValue);
                            break;
                        case DataTypeEnum.Uint32:
                            if (dataValue != null) ret.Value = uint.Parse(dataValue);
                            break;
                        case DataTypeEnum.Float:
                            if (dataValue != null) ret.Value = float.Parse(dataValue);
                            break;
                        case DataTypeEnum.Double:
                            if (dataValue != null) ret.Value = double.Parse(dataValue);
                            break;
                        case DataTypeEnum.AsciiString:
                        case DataTypeEnum.Utf8String:
                            ret.Value = dataValue;
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

        [Method("测试方法", description: "测试方法，返回当前时间")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
                ret.Value = DateTime.Now;
            else
            {
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                ret.Message = "连接失败";
            }

            return ret;
        }

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioArg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            await Task.CompletedTask;
            return rpcResponse;
        }

        #endregion

    }
}