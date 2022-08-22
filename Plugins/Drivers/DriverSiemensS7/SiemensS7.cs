using PluginInterface;
using S7.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using System;
using S7.Net.Types;

namespace DriverSiemensS7
{
    [DriverSupported("1500")]
    [DriverSupported("1200")]
    [DriverSupported("400")]
    [DriverSupported("300")]
    [DriverSupported("200")]
    [DriverSupported("200Smart")]
    [DriverInfo("SiemensS7", "V1.0.0", "Copyright IoTGateway© 2021-12-19")]
    public class SiemensS7 : IDriver
    {
        private Plc? plc;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("PLC类型")] public CpuType CpuType { get; set; } = CpuType.S71200;

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 102;

        [ConfigParameter("Rack")] public short Rack { get; set; } = 0;

        [ConfigParameter("Slot")] public short Slot { get; set; } = 0;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        public SiemensS7(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        public bool IsConnected
        {
            get { return plc != null && plc.IsConnected; }
        }

        public bool Connect()
        {
            try
            {
                plc = new Plc(CpuType, IpAddress, Port, Rack, Slot);
                plc.ReadTimeout = Timeout;
                plc.WriteTimeout = Timeout;
                plc.Open();
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
                plc?.Close();
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
                plc = null;
            }
            catch (Exception)
            {
            }
        }

        [Method("读西门子PLC标准地址", description: "读西门子PLC标准地址")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (plc != null && plc.IsConnected)
            {
                try
                {
                    if (ioarg.ValueType == DataTypeEnum.AsciiString)
                    {
                        var dataItem = S7.Net.Types.DataItem.FromAddress(ioarg.Address);
                        var head = plc.ReadBytes(dataItem.DataType, dataItem.DB, dataItem.StartByteAdr, 2);
                        var strBytes = plc.ReadBytes(dataItem.DataType, dataItem.DB, dataItem.StartByteAdr + 2, head[1]);
                        ret.Value = Encoding.ASCII.GetString(strBytes).TrimEnd(new char[] { '\0' }); ;
                    }
                    else
                        ret.Value = plc.Read(ioarg.Address);
                    if (ioarg.ValueType == DataTypeEnum.Float)
                    {
                        var buffer = new byte[4];

                        buffer[3] = (byte)((uint)ret.Value >> 24);
                        buffer[2] = (byte)((uint)ret.Value >> 16);
                        buffer[1] = (byte)((uint)ret.Value >> 8);
                        buffer[0] = (byte)((uint)ret.Value >> 0);
                        ret.Value = BitConverter.ToSingle(buffer, 0);
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

        [Method("读西门子字节字符串", description: "DB10.DBW6,10  即开始地址，字节长度")]
        public DriverReturnValueModel ReadByteString(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (plc != null && plc.IsConnected)
            {
                var str = string.Empty;
                try
                {
                    var arrParams = ioarg.Address.Trim().Split(',');
                    if (arrParams.Length ==2)
                    {
                        var dataItemitem = S7.Net.Types.DataItem.FromAddress(arrParams[0]);
                        int.TryParse(arrParams[1], out var length);

                        var data = plc.ReadBytes(dataItemitem.DataType, dataItemitem.DB, dataItemitem.StartByteAdr, length);
                        str = Encoding.ASCII.GetString(data).TrimEnd(new char[] { '\0' });
                    }
                    ret.Value = str;
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

        //预留了大小端转换的 
        private ushort[] ChangeBuffersOrder(ushort[] buffers, DataTypeEnum dataType)
        {
            var newBuffers = new ushort[buffers.Length];
            if (dataType.ToString().Contains("32") || dataType.ToString().Contains("Float"))
            {
                var a = buffers[0] & 0xff00; //A
                var b = buffers[0] & 0x00ff; //B
                var c = buffers[1] & 0xff00; //C
                var d = buffers[1] & 0x00ff; //D
                if (dataType.ToString().Contains("_1"))
                {
                    newBuffers[0] = (ushort)(a + b); //AB
                    newBuffers[1] = (ushort)(c + d); //CD
                }
                else if (dataType.ToString().Contains("_2"))
                {
                    newBuffers[0] = (ushort)((a >> 8) + (b << 8)); //BA
                    newBuffers[1] = (ushort)((c >> 8) + (d << 8)); //DC
                }
                else if (dataType.ToString().Contains("_3"))
                {
                    newBuffers[0] = (ushort)((c >> 8) + (d << 8)); //DC
                    newBuffers[1] = (ushort)((a >> 8) + (b << 8)); //BA
                }
                else
                {
                    newBuffers[0] = (ushort)(c + d); //CD
                    newBuffers[1] = (ushort)(a + b); //AB
                }
            }
            else if (dataType.ToString().Contains("64") || dataType.ToString().Contains("Double"))
            {
                if (dataType.ToString().Contains("_1"))
                {
                }
                else
                {
                    newBuffers[0] = buffers[3];
                    newBuffers[1] = buffers[2];
                    newBuffers[2] = buffers[1];
                    newBuffers[3] = buffers[0];
                }
            }
            else
            {
                if (dataType.ToString().Contains("_1"))
                {
                    var h8 = buffers[0] & 0xf0;
                    var l8 = buffers[0] & 0x0f;
                    newBuffers[0] = (ushort)(h8 >> 8 + l8 << 8);
                }
                else
                    newBuffers[0] = buffers[0];
            }

            return newBuffers;
        }

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false };
            try
            {
                if (!IsConnected)
                    rpcResponse.Description = "设备连接已断开";
                else
                {
                    object? toWrite = null;
                    switch (ioarg.ValueType)
                    {
                        case DataTypeEnum.Bit:
                        case DataTypeEnum.Bool:
                            toWrite = ioarg.Value.ToString()?.ToLower() == "true" ||
                                      ioarg.Value.ToString()?.ToLower() == "1";
                            break;
                        case DataTypeEnum.UByte:
                            toWrite = byte.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Byte:
                            toWrite = sbyte.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Uint16:
                            toWrite = ushort.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Int16:
                            toWrite = short.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Uint32:
                            toWrite = uint.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Int32:
                            toWrite = int.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.Float:
                            toWrite = float.Parse(ioarg.Value.ToString());
                            break;
                        case DataTypeEnum.AsciiString:
                            toWrite = GetStringBytes(ioarg);
                            break;
                        default:
                            rpcResponse.Description = $"类型{DataTypeEnum.Float}不支持写入";
                            break;
                    }

                    if (toWrite == null)
                    {
                        rpcResponse.Description = "解析错误";
                        return rpcResponse;
                    }

                    //通用方法
                    if (method == nameof(Read))
                    {
                        plc?.Write(ioarg.Address, toWrite);

                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }
                    //字符串
                    else if (method == nameof(ReadByteString))
                    {
                        var arrParams = ioarg.Address.Trim().Split(',');
                        if (arrParams.Length == 2)
                        {
                            var dataItem = DataItem.FromAddress(arrParams[0]);
                            plc?.Write(dataItem.DataType, dataItem.DB, dataItem.StartByteAdr, toWrite);
                            rpcResponse.IsSuccess = true;
                            return rpcResponse;
                        }
                    }
                    else
                        rpcResponse.Description = $"不支持写入:{method}";
                }
            }
            catch (Exception ex)
            {
                rpcResponse.Description = $"写入失败,[method]:{method},[ioarg]:{ioarg},[ex]:{ex}";
            }

            return rpcResponse;
        }

        private byte[]? GetStringBytes(DriverAddressIoArgModel ioarg)
        {
            var toWriteString = ioarg.Value.ToString();
            try
            {
                var arrParams = ioarg.Address.Trim().Split(',');
                int length = 0;//最大长度，因为字符串后面得补满'\0'
                //直接读取byte[]的方式
                if (arrParams.Length == 2)
                {
                    //如DB100.DBW23,10
                    int.TryParse(arrParams[1], out length);
                }
                //使用西门子String读取
                else
                {
                    //如DB100.DBW23
                    var dataItem = DataItem.FromAddress(ioarg.Address);
                    var head = plc.ReadBytes(dataItem.DataType, dataItem.DB, dataItem.StartByteAdr, 2);
                    length = head[0];
                }

                if (toWriteString.Length > length)
                    toWriteString = toWriteString.Take(length).ToString();
                if (toWriteString.Length < length)
                    toWriteString = toWriteString.PadRight(length, '\0');
            }
            catch (Exception e)
            {
                throw new Exception("字符串解析异常");
            }

            return Encoding.ASCII.GetBytes(toWriteString);
        }
    }
}