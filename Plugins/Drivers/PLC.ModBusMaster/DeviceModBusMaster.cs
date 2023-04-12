using System.Text;
using Modbus.Device;
using Modbus.Serial;
using PluginInterface;
using System.IO.Ports;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace PLC.ModBusMaster
{
    [DriverSupported("TCP")]
    [DriverSupported("UDP")]
    [DriverSupported("Rtu")]
    [DriverSupported("Rtu Over TCP")]
    [DriverSupported("Rtu Over UDP")]
    [DriverSupported("Ascii")]
    [DriverSupported("Ascii Over TCP")]
    [DriverSupported("Ascii Over UDP")]
    [DriverInfo("ModBusMaster", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceModBusMaster : IDriver
    {
        private TcpClient? _tcpClient;
        private UdpClient? _udpClient;
        private SerialPort? _serialPort;
        private ModbusMaster? _master;
        private SerialPortAdapter? _adapter;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("主站类型")] public MasterType MasterType { get; set; } = MasterType.Tcp;

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 502;

        [ConfigParameter("串口名")] public string PortName { get; set; } = "COM1";

        [ConfigParameter("波特率")] public int BaudRate { get; set; } = 9600;

        [ConfigParameter("数据位")] public int DataBits { get; set; } = 8;

        [ConfigParameter("校验位")] public Parity Parity { get; set; } = Parity.None;

        [ConfigParameter("停止位")] public StopBits StopBits { get; set; } = StopBits.One;

        [ConfigParameter("从站号")] public byte SlaveAddress { get; set; } = 1;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        #region 生命周期

        /// <summary>
        /// 反射构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="logger"></param>
        public DeviceModBusMaster(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                switch (MasterType)
                {
                    case MasterType.Tcp:
                    case MasterType.RtuOnTcp:
                    case MasterType.AsciiOnTcp:
                        return _tcpClient != null && _master != null && _tcpClient.Connected;
                    case MasterType.Udp:
                    case MasterType.RtuOnUdp:
                    case MasterType.AsciiOnUdp:
                        return _udpClient != null && _master != null && _udpClient.Client.Connected;
                    case MasterType.Rtu:
                    case MasterType.Ascii:
                        return _serialPort != null && _master != null && _serialPort.IsOpen;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");
                switch (MasterType)
                {
                    case MasterType.Tcp:
                        _tcpClient = new TcpClient(IpAddress, Port);
                        _tcpClient.ReceiveTimeout = Timeout;
                        _tcpClient.SendTimeout = Timeout;
                        _master = ModbusIpMaster.CreateIp(_tcpClient);
                        break;
                    case MasterType.Udp:
                        _udpClient = new UdpClient(IpAddress, Port);
                        _udpClient.Client.ReceiveTimeout = Timeout;
                        _udpClient.Client.SendTimeout = Timeout;
                        _master = ModbusIpMaster.CreateIp(_udpClient);
                        break;
                    case MasterType.Rtu:
                        _serialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                        _serialPort.ReadTimeout = Timeout;
                        _serialPort.WriteTimeout = Timeout;
                        _serialPort.Open();
                        _adapter = new SerialPortAdapter(_serialPort);
                        _master = ModbusSerialMaster.CreateRtu(_adapter);
                        break;
                    case MasterType.RtuOnTcp:
                        _tcpClient = new TcpClient(IpAddress, Port);
                        _tcpClient.ReceiveTimeout = Timeout;
                        _tcpClient.SendTimeout = Timeout;
                        _master = ModbusSerialMaster.CreateRtu(_tcpClient);
                        break;
                    case MasterType.RtuOnUdp:
                        _udpClient = new UdpClient(IpAddress, Port);
                        _udpClient.Client.ReceiveTimeout = Timeout;
                        _udpClient.Client.SendTimeout = Timeout;
                        _master = ModbusSerialMaster.CreateRtu(_udpClient);
                        break;
                    case MasterType.Ascii:
                        _serialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                        _serialPort.ReadTimeout = Timeout;
                        _serialPort.WriteTimeout = Timeout;
                        _serialPort.Open();
                        _adapter = new SerialPortAdapter(_serialPort);
                        _master = ModbusSerialMaster.CreateAscii(_adapter);
                        break;
                    case MasterType.AsciiOnTcp:
                        _tcpClient = new TcpClient(IpAddress, Port);
                        _tcpClient.ReceiveTimeout = Timeout;
                        _tcpClient.SendTimeout = Timeout;
                        _master = ModbusSerialMaster.CreateAscii(_tcpClient);
                        break;
                    case MasterType.AsciiOnUdp:
                        _udpClient = new UdpClient(IpAddress, Port);
                        _udpClient.Client.ReceiveTimeout = Timeout;
                        _udpClient.Client.SendTimeout = Timeout;
                        _master = ModbusSerialMaster.CreateAscii(_udpClient);
                        break;
                }

                _master!.Transport.ReadTimeout = Timeout;
                _master!.Transport.WriteTimeout = Timeout;
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
                _tcpClient?.Close();
                _udpClient?.Close();
                _serialPort?.Close();
                _logger.LogInformation($"Device:[{_device}],Close()");
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
                _tcpClient?.Dispose();
                _udpClient?.Dispose();
                _serialPort?.Dispose();
                _master?.Dispose();

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

        /// <summary>
        /// 批量读后缓存
        /// </summary>
        private Dictionary<string, object> _cache = new();
        [Method("多地址读取", description: "多地址读取缓存")]
        public DriverReturnValueModel ReadMultiple(DriverAddressIoArgModel ioArg)
        {
            byte slaveId = SlaveAddress;
            if (ioArg.Address.Contains('|'))
            {
                slaveId = byte.Parse(ioArg.Address.Split('|')[0]);
                ioArg.Address = ioArg.Address.Split('|')[1];
            }
            var args = ioArg.Address.Split(',');
            var func = args[0];//功能码
            var startAddress = ushort.Parse(args[1]);//开始地址
            var length = ushort.Parse(args[2]);//读取字数
            var cacheKey = args[3];//缓存字典名
            try
            {

                switch (func)
                {
                    case "1":
                        var coils = _master.ReadCoils(slaveId, startAddress, length);
                        _cache[cacheKey] = coils;
                        break;
                    case "2":
                        var inputs = _master.ReadInputs(slaveId, startAddress, length);
                        _cache[cacheKey] = inputs;
                        break;
                    case "3":
                        var holdingRs = _master.ReadHoldingRegisters(slaveId, startAddress, length);
                        _cache[cacheKey] = holdingRs;
                        break;
                    case "4":
                        var inputRs = _master.ReadInputRegisters(slaveId, startAddress, length);
                        _cache[cacheKey] = inputRs;
                        break;
                }

                return new DriverReturnValueModel()
                {
                    StatusType = VaribaleStatusTypeEnum.Good,
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(_cache[cacheKey])
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],ReadMultiple(),Error");
                _cache[cacheKey] = null;
                return new DriverReturnValueModel()
                {
                    StatusType = VaribaleStatusTypeEnum.Bad
                };
            }

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
                string cacheName = _cache.Any() ? _cache.FirstOrDefault().Key : "0";
                int startIndex = 0;
                if (ioArg.Address.Contains(","))
                {
                    var args = ioArg.Address.Split(',');
                    cacheName = args[0];
                    startIndex = int.Parse(args[1]);
                }
                else
                {
                    startIndex = int.Parse(ioArg.Address);
                }


                if (_cache.ContainsKey(cacheName) && _cache[cacheName] != null)
                {
                    var cacheBuffers = (ushort[])_cache[cacheName];
                    var rawBuffers = cacheBuffers.Skip(startIndex).Take(cacheBuffers.Length - startIndex).ToArray();


                    var retBuffers = ChangeBuffersOrder(rawBuffers, ioArg.EndianType);
                    if (ioArg.ValueType == DataTypeEnum.AsciiString)
                        retBuffers = rawBuffers;

                    if (ioArg.ValueType.ToString().Contains("Uint16"))
                        ret.Value = retBuffers[0];
                    else if (ioArg.ValueType.ToString().Contains("Int16"))
                        ret.Value = (short)retBuffers[0];
                    else if (ioArg.ValueType.ToString().Contains("Bcd16"))
                        ret.Value = ModBusDataConvert.GetBCD(GetBytes(retBuffers));
                    else if (ioArg.ValueType.ToString().Contains("Uint32"))
                        ret.Value = (uint)(retBuffers[0] << 16) + retBuffers[1];
                    else if (ioArg.ValueType.ToString().Contains("Int32"))
                        ret.Value = (retBuffers[0] << 16) + retBuffers[1];
                    else if (ioArg.ValueType.ToString().Contains("Bcd32"))
                    {
                        var newBuffers = new ushort[2] { retBuffers[1], retBuffers[0] };
                        ret.Value = ModBusDataConvert.GetBCD(GetBytes(newBuffers));
                    }
                    else if (ioArg.ValueType.ToString().Contains("Float"))
                    {
                        var bytes = new[]
                        {
                            (byte)(retBuffers[1] & 0xff), (byte)((retBuffers[1] >> 8) & 0xff),
                            (byte)(retBuffers[0] & 0xff), (byte)((retBuffers[0] >> 8) & 0xff)
                        };
                        ret.Value = BitConverter.ToSingle(bytes, 0);
                    }
                    else if (ioArg.ValueType.ToString().Contains("AsciiString"))
                    {
                        var str = Encoding.ASCII.GetString(GetBytes(retBuffers).ToArray());
                        if (str.Contains('\0'))
                            str = str.Split('\0')[0];
                        ret.Value = str;
                    }

                    ret.StatusType = VaribaleStatusTypeEnum.Good;
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
        /// 功能码03
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("功能码:03", description: "HoldingRegisters读保持寄存器")]
        public DriverReturnValueModel HoldingRegisters(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                    ret = ReadRegistersBuffers(3, ioArg);
                else
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = "TCP连接异常";
                }
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.UnKnow;
                ret.Message = ex.Message;
                _logger.LogError(ex, $"Device:[{_device}],ReadHoldingRegisters(),Error");
            }

            return ret;
        }

        /// <summary>
        /// 功能码04
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("功能码:04", description: "HoldingRegisters读输入寄存器")]
        public DriverReturnValueModel InputRegisters(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                    ret = ReadRegistersBuffers(4, ioArg);
                else
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = "TCP连接异常";
                }
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.UnKnow;
                ret.Message = ex.Message;
                _logger.LogError(ex, $"Device:[{_device}],ReadInputRegisters(),Error");
            }

            return ret;
        }

        /// <summary>
        /// 功能码01
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("功能码:01", description: "Coil读线圈")]
        public DriverReturnValueModel Coil(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                {
                    var (slaveAddress, ioAddress) = GetSlaveAddress(ioArg);
                    var retBool = _master.ReadCoils(slaveAddress, ushort.Parse(ioAddress), 1)[0];
                    if (ioArg.ValueType == DataTypeEnum.Bit)
                    {
                        if (retBool)
                            ret.Value = 1;
                        else
                            ret.Value = 0;
                    }
                    else
                        ret.Value = retBool;

                    ret.StatusType = VaribaleStatusTypeEnum.Good;
                }
                else
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = "TCP连接异常";
                }
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.UnKnow;
                ret.Message = ex.Message;
                _logger.LogError(ex, $"Device:[{_device}],ReadCoil(),Error");
            }

            return ret;
        }

        /// <summary>
        /// 功能码02
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        [Method("功能码:02", description: "Input读输入")]
        public DriverReturnValueModel Input(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                {
                    var (slaveAddress, ioAddress) = GetSlaveAddress(ioArg);
                    var retBool = _master.ReadInputs(slaveAddress, ushort.Parse(ioAddress), 1)[0];
                    if (ioArg.ValueType == DataTypeEnum.Bit)
                    {
                        if (retBool)
                            ret.Value = 1;
                        else
                            ret.Value = 0;
                    }
                    else
                        ret.Value = retBool;

                    ret.StatusType = VaribaleStatusTypeEnum.Good;
                }
                else
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = "TCP连接异常";
                }
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.UnKnow;
                ret.Message = ex.Message;
                _logger.LogError(ex, $"Device:[{_device}],ReadInput(),Error");
            }

            return ret;
        }


        [Method("Read方法样例", description: "Read方法样例描述")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new DriverReturnValueModel
            {
                Message = "",
                StatusType = VaribaleStatusTypeEnum.Good,
                Value = $"{DeviceId} {DateTime.Now.ToString("O")} Read {ioArg.Address}"
            };
            return ret;
        }



        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="method"></param>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioArg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false };
            try
            {
                if (!IsConnected)
                    rpcResponse.Description = "设备连接已断开";
                else
                {
                    ushort address, count;
                    AnalyzeAddress(ioArg, out address, out count);
                    var (slaveAddress, ioAddress) = GetSlaveAddress(ioArg);

                    //功能码01
                    if (method == nameof(Coil))
                    {
                        var value = ioArg.Value.ToString() == "1" || ioArg.Value.ToString().ToLower() == "true";
                        await _master.WriteSingleCoilAsync(slaveAddress, address, value);
                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }

                    //功能码03
                    if (method == nameof(HoldingRegisters))
                    {
                        ushort[] shortArray = new ushort[count];

                        switch (ioArg.ValueType)
                        {
                            case DataTypeEnum.AsciiString:
                                ModBusDataConvert.SetString(shortArray, 0, ioArg.Value.ToString());
                                await _master.WriteMultipleRegistersAsync(slaveAddress, address, shortArray);

                                break;
                            case DataTypeEnum.Float:
                                float f = 0;
                                float.TryParse(ioArg.Value.ToString(), out f);
                                ModBusDataConvert.SetReal(shortArray, 0, f);
                                await _master.WriteMultipleRegistersAsync(slaveAddress, address, shortArray);

                                break;
                            default:
                                await _master.WriteSingleRegisterAsync(slaveAddress, address,
                                    ushort.Parse(ioArg.Value.ToString()));
                                break;
                        }

                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }

                    rpcResponse.Description = $"不支持写入:{method}";
                }
            }
            catch (Exception ex)
            {
                rpcResponse.Description = $"写入失败,[method]:{method},[ioArg]:{ioArg},[ex]:{ex}";
                _logger.LogError(ex, $"Device:[{_device}],WriteAsync(),Error");
            }

            return rpcResponse;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 读功能码03、或04
        /// </summary>
        /// <param name="funCode"></param>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        private DriverReturnValueModel ReadRegistersBuffers(byte funCode, DriverAddressIoArgModel ioArg)
        {
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            if (!IsConnected)
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
            else
            {
                ushort startAddress, count;
                ret = AnalyzeAddress(ioArg, out startAddress, out count);
                var (slaveAddress, ioAddress) = GetSlaveAddress(ioArg);

                if (ret.StatusType != VaribaleStatusTypeEnum.Good)
                    return ret;
                try
                {
                    var rawBuffers = new ushort[] { };
                    if (funCode == 3)
                        rawBuffers = _master.ReadHoldingRegisters(slaveAddress, startAddress, count);
                    else if (funCode == 4)
                        rawBuffers = _master.ReadInputRegisters(slaveAddress, startAddress, count);

                    var retBuffers = ChangeBuffersOrder(rawBuffers, ioArg.EndianType);
                    if (ioArg.ValueType == DataTypeEnum.AsciiString)
                        retBuffers = rawBuffers;

                    if (ioArg.ValueType.ToString().Contains("Uint16"))
                        ret.Value = retBuffers[0];
                    else if (ioArg.ValueType.ToString().Contains("Int16"))
                        ret.Value = (short)retBuffers[0];
                    else if (ioArg.ValueType.ToString().Contains("Bcd16"))
                        ret.Value = ModBusDataConvert.GetBCD(GetBytes(retBuffers));
                    else if (ioArg.ValueType.ToString().Contains("Uint32"))
                        ret.Value = (uint)(retBuffers[0] << 16) + retBuffers[1];
                    else if (ioArg.ValueType.ToString().Contains("Int32"))
                        ret.Value = (retBuffers[0] << 16) + retBuffers[1];
                    else if (ioArg.ValueType.ToString().Contains("Bcd32"))
                    {
                        var newBuffers = new ushort[2] { retBuffers[1], retBuffers[0] };
                        ret.Value = ModBusDataConvert.GetBCD(GetBytes(newBuffers));
                    }
                    else if (ioArg.ValueType.ToString().Contains("Float"))
                    {
                        var bytes = new[]
                        {
                            (byte)(retBuffers[1] & 0xff), (byte)((retBuffers[1] >> 8) & 0xff),
                            (byte)(retBuffers[0] & 0xff), (byte)((retBuffers[0] >> 8) & 0xff)
                        };
                        ret.Value = BitConverter.ToSingle(bytes, 0);
                    }
                    else if (ioArg.ValueType.ToString().Contains("AsciiString"))
                    {
                        var str = Encoding.ASCII.GetString(GetBytes(retBuffers).ToArray());
                        if (str.Contains('\0'))
                            str = str.Split('\0')[0];
                        ret.Value = str;
                    }
                }
                catch (Exception ex)
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = ex.Message;
                    _logger.LogError(ex, $"Device:[{_device}],ReadRegistersBuffers(),Error");
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionCode"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private ushort GetModbusReadCount(uint functionCode, DataTypeEnum dataType)
        {
            if (dataType.ToString().Contains("32") || dataType.ToString().Contains("Float"))
                return 2;
            if (dataType.ToString().Contains("64") || dataType.ToString().Contains("Double"))
                return 4;
            return 1;
        }

        /// <summary>
        /// 大小端转换
        /// </summary>
        /// <param name="buffers"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private ushort[] ChangeBuffersOrder(ushort[] buffers, EndianEnum dataType)
        {
            int datalen = buffers.Length;
            ushort[] newBuffers = new ushort[datalen];
            if (dataType == EndianEnum.None)
            {
                newBuffers = buffers;
            }
            else
            {

                if (datalen == 1)//16位
                {
                    switch (dataType)
                    {
                        case EndianEnum.LittleEndian://BA
                            var ab = BitConverter.GetBytes(buffers[0]);
                            newBuffers[0] = BitConverter.ToUInt16(new byte[] { ab[1], ab[0] });
                            break;
                        case EndianEnum.BigEndian://AB
                        default:
                            newBuffers[0] = buffers[0];
                            break;
                    }
                }
                else if (datalen == 2)//32位 
                {
                    newBuffers = new ushort[2];
                    var ab = BitConverter.GetBytes(buffers[0]);
                    var cd = BitConverter.GetBytes(buffers[1]);
                    var _ab = new byte[2];
                    var _cd = new byte[2];
                    switch (dataType)
                    {
                        case EndianEnum.BigEndian://ABCD
                            _ab = ab;
                            _cd = cd;
                            break;
                        case EndianEnum.LittleEndian://DCBA
                            _ab[0] = cd[1];
                            _ab[1] = cd[0];
                            _cd[0] = ab[1];
                            _cd[1] = ab[0];
                            break;
                        case EndianEnum.BigEndianSwap://BADC
                            _ab[0] = ab[1];
                            _ab[1] = ab[0];
                            _cd[0] = cd[1];
                            _cd[1] = cd[0];
                            break;
                        case EndianEnum.LittleEndianSwap://CDAB
                            _ab[0] = cd[0];
                            _ab[1] = cd[1];
                            _cd[0] = ab[0];
                            _cd[1] = ab[1];
                            break;
                        default:
                            break;
                    }
                    newBuffers[0] = BitConverter.ToUInt16(_ab, 0);
                    newBuffers[1] = BitConverter.ToUInt16(_cd, 0);
                }
                else if (datalen == 4)//64位
                {
                    newBuffers = new ushort[2];
                    var ab = BitConverter.GetBytes(buffers[0]);
                    var cd = BitConverter.GetBytes(buffers[1]);
                    var ef = BitConverter.GetBytes(buffers[2]);
                    var gh = BitConverter.GetBytes(buffers[3]);
                    var _ab = new byte[2];
                    var _cd = new byte[2];
                    var _ef = new byte[2];
                    var _gh = new byte[2];
                    switch (dataType)
                    {
                        case EndianEnum.BigEndian://AB CD EF GH
                            _ab = ab;
                            _cd = cd;
                            _ef = ef;
                            _gh = gh;
                            break;
                        case EndianEnum.LittleEndian://HG FE DC BA
                            _ab[0] = gh[1];
                            _ab[1] = gh[0];
                            _cd[0] = ef[1];
                            _cd[1] = ef[0];

                            _ef[0] = cd[1];
                            _ef[1] = cd[0];
                            _gh[0] = ab[1];
                            _gh[1] = ab[0];
                            break;
                        case EndianEnum.BigEndianSwap://BA DC FE HG
                            _ab[0] = ab[1];
                            _ab[1] = ab[0];
                            _cd[0] = cd[1];
                            _cd[1] = cd[0];

                            _ef[0] = ef[1];
                            _ef[1] = ef[0];
                            _gh[0] = gh[1];
                            _gh[1] = gh[0];
                            break;
                        case EndianEnum.LittleEndianSwap://GH EF CD AB
                            _ab[0] = gh[0];
                            _ab[1] = gh[1];
                            _cd[0] = ef[0];
                            _cd[1] = ef[1];

                            _ef[0] = cd[0];
                            _ef[1] = cd[1];
                            _gh[0] = ab[0];
                            _gh[1] = ab[1];
                            break;
                        default:
                            break;
                    }
                    newBuffers[0] = BitConverter.ToUInt16(_ab, 0);
                    newBuffers[1] = BitConverter.ToUInt16(_cd, 0);
                    newBuffers[2] = BitConverter.ToUInt16(_ef, 0);
                    newBuffers[3] = BitConverter.ToUInt16(_gh, 0);
                }
                else
                {
                    newBuffers = buffers;
                }
            }
            return newBuffers;
        }

        /// <summary>
        /// ushort数组转byte数组
        /// </summary>
        /// <param name="retBuffers"></param>
        /// <returns></returns>
        private List<byte> GetBytes(ushort[] retBuffers)
        {
            List<byte> vs = new();
            foreach (var retBuffer in retBuffers)
            {
                vs.Add((byte)(retBuffer & 0xFF));
                vs.Add((byte)((retBuffer & 0xFF00) >> 8));
            }

            return vs;
        }

        /// <summary>
        /// 解析出特定站号
        /// </summary>
        /// <param name="ioArg"></param>
        /// <param name="startAddress"></param>
        /// <param name="readCount"></param>
        /// <returns></returns>
        private DriverReturnValueModel AnalyzeAddress(DriverAddressIoArgModel ioArg, out ushort startAddress,
            out ushort readCount)
        {
            var (slaveAddress, ioAddress) = GetSlaveAddress(ioArg);
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            try
            {
                if (ioArg.ValueType == DataTypeEnum.AsciiString)
                {
                    startAddress = ushort.Parse(ioAddress.Split(',')[0]);
                    readCount = ushort.Parse(ioAddress.Split(',')[1]);
                }
                else
                {
                    startAddress = ushort.Parse(ioAddress);
                    readCount = GetModbusReadCount(3, ioArg.ValueType);
                }

                return ret;
            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.AddressError;
                ret.Message = ex.Message;
                startAddress = 0;
                readCount = 0;
                _logger.LogError(ex, $"Device:[{_device}],AnalyzeAddress(),Error");
                return ret;
            }
        }

        /// <summary>
        /// 支持灵活读取不同SlaveId的数据
        /// </summary>
        /// <param name="ioArg"></param>
        /// <returns></returns>
        private (byte slaveAddress, string ioAddress) GetSlaveAddress(DriverAddressIoArgModel ioArg)
        {
            byte slaveAddress = SlaveAddress;
            string ioAddress = ioArg.Address;
            try
            {
                if (ioArg.Address.Contains('|'))
                {
                    slaveAddress = byte.Parse(ioArg.Address.Split('|')[0]);
                    ioAddress = ioArg.Address.Split('|')[1];
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Device:[{_device}],AnalyseAddress(),Error");
            }

            return (slaveAddress, ioAddress);
        }
        #endregion

    }
}