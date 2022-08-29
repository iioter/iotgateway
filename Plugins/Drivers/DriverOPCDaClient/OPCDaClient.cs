using PluginInterface;
using Automation.OPCClient;
using Microsoft.Extensions.Logging;

namespace DriverOPCDaClient
{
    [DriverSupported("OPCDaClient")]
    [DriverInfo("OPCDaClient", "V1.0.0", "Copyright IoTGateway© 2022-08-10")]
    internal class OPCDaClient : IDriver
    {
        private OPCClientWrapper? opcDaClient;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP")] public string Ip { get; set; } = "127.0.0.1";

        [ConfigParameter("OpcServerName")] public string OpcServerName { get; set; } = "ICONICS.SimulatorOPCDA.2";

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        public OPCDaClient(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }


        public bool IsConnected
        {
            get { return opcDaClient != null && opcDaClient.IsOPCServerConnected(); }
        }

        public bool Connect()
        {
            try
            {
                opcDaClient = new OPCClientWrapper();
                opcDaClient.Init(Ip, OpcServerName);
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
                opcDaClient?.Disconnect();
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
                opcDaClient = null;
            }
            catch (Exception)
            {
            }
        }


        [Method("读OPCDa", description: "读OPCDa节点")]
        public DriverReturnValueModel ReadNode(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    var dataValue = opcDaClient?.ReadNodeLabel(ioarg.Address);
                    switch (ioarg.ValueType)
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

        [Method("测试方法", description: "测试方法，返回当前时间")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
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

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return rpcResponse;
        }
    }
}