using IoTClient.Enums;
using PluginInterface;
using IoTClient.Clients.PLC;
using Microsoft.Extensions.Logging;
using DataTypeEnum = IoTClient.Enums.DataTypeEnum;
using System.Text;
using System.Linq;

namespace PLC.MelsecMc
{
    [DriverSupported("A_1E")]
    [DriverSupported("Qna_3E")]
    [DriverInfo("MelsecMc", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceMelsecMc : IDriver
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

        #region 生命周期

        /// <summary>
        /// 反射构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="logger"></param>
        public DeviceMelsecMc(string device, ILogger logger)
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

                _plc = new MitsubishiClient(CpuType, IpAddress, Port);
                _plc.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Connect(),Error");
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
                        case PluginInterface.DataTypeEnum.Bit:
                            ret.Value = _plc.ReadBoolean(ioArg.Address).Value ? 1 : 0;
                            break;
                        case PluginInterface.DataTypeEnum.Bool:
                            ret.Value = _plc.ReadBoolean(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.UByte:
                            ret.Value = _plc.ReadByte(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Byte:
                            ret.Value = (sbyte)_plc.ReadByte(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint16:
                            ret.Value = _plc.ReadUInt16(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int16:
                            ret.Value = _plc.ReadInt16(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint32:
                            ret.Value = _plc.ReadUInt32(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int32:
                            ret.Value = _plc.ReadInt32(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Float:
                            ret.Value = _plc.ReadFloat(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Double:
                            ret.Value = _plc.ReadDouble(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint64:
                            ret.Value = _plc.ReadUInt64(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int64:
                            ret.Value = _plc.ReadInt64(ioArg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.AsciiString:
                            ret.Value = _plc.ReadString(ioArg.Address);
                            break;
                        case PluginInterface.DataTypeEnum.Utf8String:
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


        private Dictionary<string, object> _cache = new();
        /// <summary>
        /// 读
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("批量读PLC标准地址", description: "数据区,开始地址,地址间隔,读取总数")]
        public DriverReturnValueModel ReadMultiple(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (_plc != null && this.IsConnected)
            {
                try
                {
                    var addresStrings = ioArg.Address.Split(',');
                    string block = addresStrings[0];
                    ushort start = ushort.Parse(addresStrings[1]);
                    ushort step = ushort.Parse(addresStrings[2]);
                    ushort count = ushort.Parse(addresStrings[3]);

                    var dataType = DataTypeEnum.UInt16;

                    switch (ioArg.ValueType)
                    {
                        case PluginInterface.DataTypeEnum.Bool:
                            dataType = DataTypeEnum.Bool;
                            break;
                        case PluginInterface.DataTypeEnum.Bit:
                        case PluginInterface.DataTypeEnum.UByte:
                        case PluginInterface.DataTypeEnum.Byte:
                            dataType = DataTypeEnum.Byte;
                            break;
                        case PluginInterface.DataTypeEnum.Uint16:
                            dataType = DataTypeEnum.UInt16;
                            break;
                        case PluginInterface.DataTypeEnum.Int16:
                            dataType = DataTypeEnum.Int16;
                            break;
                        case PluginInterface.DataTypeEnum.Uint32:
                            dataType = DataTypeEnum.UInt32;
                            break;
                        case PluginInterface.DataTypeEnum.Int32:
                            dataType = DataTypeEnum.Int32;
                            break;
                        case PluginInterface.DataTypeEnum.Float:
                            dataType = DataTypeEnum.Float;
                            break;
                        case PluginInterface.DataTypeEnum.Double:
                            dataType = DataTypeEnum.Double;
                            break;
                        case PluginInterface.DataTypeEnum.Uint64:
                            dataType = DataTypeEnum.UInt64;
                            break;
                        case PluginInterface.DataTypeEnum.Int64:
                            dataType = DataTypeEnum.Int64;
                            break;
                        case PluginInterface.DataTypeEnum.AsciiString:
                            dataType = DataTypeEnum.String;
                            break;
                        case PluginInterface.DataTypeEnum.Utf8String:
                            dataType = DataTypeEnum.String;
                            break;
                        default:
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败,不支持的类型:{ioArg.ValueType}";
                            break;
                    }

                    var batchAddress = new Dictionary<string, DataTypeEnum>();
                    for (int i = 0; i < count; i++)
                    {
                        batchAddress[$"{block}{start + i * step}"] = dataType;
                    }

                    try
                    {
                        var batchResult = _plc.BatchRead(batchAddress, count);

                        if (batchResult.IsSucceed)
                        {
                            foreach (var value in batchResult.Value)
                            {
                                _cache[value.Key] = value.Value;
                            }

                            ret.Value = batchResult.Value.Select(x => x.Value).ToList();
                        }
                        else
                        {
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            foreach (var address in batchAddress)
                            {
                                if (_cache.ContainsKey(address.Key))
                                    _cache.Remove(address.Key);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        foreach (var address in batchAddress)
                        {
                            if (_cache.ContainsKey(address.Key))
                                _cache.Remove(address.Key);
                        }
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
        /// 从缓存内读
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("从缓存读取", description: "从缓存读取")]
        public DriverReturnValueModel ReadFromCache(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (_cache.ContainsKey(ioArg.Address) && _cache[ioArg.Address] != null)
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Good;
                    ret.Value = _cache[ioArg.Address];
                }
                else
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                }
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                _logger.LogError(ex, $"Device:[{_device}],ReadFromCache(),Error");
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