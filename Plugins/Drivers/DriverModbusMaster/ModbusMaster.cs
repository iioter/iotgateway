using Microsoft.Extensions.Logging;
using Modbus.Device;
using Modbus.Serial;
using PluginInterface;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;

namespace DriverModbusMaster
{
    [DriverSupported("ModbusTCP")]
    [DriverSupported("ModbusUDP")]
    [DriverSupported("ModbusRtu")]
    [DriverSupported("ModbusAscii")]
    [DriverInfo("ModbusMaster", "V1.1.0", "Copyright IoTGateway© 2022-8-6")]
    public class ModbusMaster : IDriver
    {
        private TcpClient? _tcpClient;
        private UdpClient? _udpClient;
        private SerialPort? _serialPort;
        private Modbus.Device.ModbusMaster? _master;
        private SerialPortAdapter? _adapter;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("PLC类型")] public PlcType PlcType { get; set; } = PlcType.S71200;

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

        public ModbusMaster(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }

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

                _master.Transport.ReadTimeout = Timeout;
                _master.Transport.WriteTimeout = Timeout;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Connect(),Error");
                return false;
            }

            return IsConnected;
        }

        public bool Close()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Close()");
                _tcpClient?.Close();
                _udpClient?.Close();
                _serialPort?.Close();
                return !IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Close(),Error");
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                _tcpClient?.Dispose();
                _udpClient?.Dispose();
                _serialPort?.Dispose();
                _master?.Dispose();
                _logger.LogInformation($"Device:[{_device}],Dispose()");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Dispose(),Error");
            }
        }

        [Method("功能码:03", description: "ReadHoldingRegisters读保持寄存器")]
        public DriverReturnValueModel ReadHoldingRegisters(DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                    ret = ReadRegistersBuffers(3, ioarg);
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


        [Method("功能码:04", description: "ReadHoldingRegisters读输入寄存器")]
        public DriverReturnValueModel ReadInputRegisters(DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                    ret = ReadRegistersBuffers(4, ioarg);
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


        [Method("功能码:01", description: "ReadCoil读线圈")]
        public DriverReturnValueModel ReadCoil(DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                {
                    var retBool = _master.ReadCoils(SlaveAddress, ushort.Parse(ioarg.Address), 1)[0];
                    if (ioarg.ValueType == DataTypeEnum.Bit)
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

        [Method("功能码:02", description: "ReadInput读输入")]
        public DriverReturnValueModel ReadInput(DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new();
            try
            {
                if (IsConnected)
                {
                    var retBool = _master.ReadInputs(SlaveAddress, ushort.Parse(ioarg.Address), 1)[0];
                    if (ioarg.ValueType == DataTypeEnum.Bit)
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
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new DriverReturnValueModel
            {
                Message = "",
                StatusType = VaribaleStatusTypeEnum.Good,
                Value = $"{DeviceId} {DateTime.Now.ToString("O")} Read {ioarg.Address}"
            };
            return ret;
        }

        //读功能码03、或04
        private DriverReturnValueModel ReadRegistersBuffers(byte funCode, DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            if (!IsConnected)
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
            else
            {
                ushort startAddress, count;
                ret = AnalyzeAddress(ioarg, out startAddress, out count);
                if (ret.StatusType != VaribaleStatusTypeEnum.Good)
                    return ret;
                try
                {
                    var rawBuffers = new ushort[] { };
                    if (funCode == 3)
                        rawBuffers = _master.ReadHoldingRegisters(SlaveAddress, startAddress, count);
                    else if (funCode == 4)
                        rawBuffers = _master.ReadInputRegisters(SlaveAddress, startAddress, count);

                    var retBuffers = ChangeBuffersOrder(rawBuffers, ioarg.ValueType);
                    if (ioarg.ValueType == DataTypeEnum.AsciiString)
                        retBuffers = rawBuffers;

                    if (ioarg.ValueType.ToString().Contains("Uint16"))
                        ret.Value = retBuffers[0];
                    else if (ioarg.ValueType.ToString().Contains("Int16"))
                        ret.Value = (short)retBuffers[0];
                    else if (ioarg.ValueType.ToString().Contains("Uint32"))
                        ret.Value = (uint)(retBuffers[0] << 16) + retBuffers[1];
                    else if (ioarg.ValueType.ToString().Contains("Int32"))
                        ret.Value = (retBuffers[0] << 16) + retBuffers[1];
                    else if (ioarg.ValueType.ToString().Contains("Float"))
                    {
                        var bytes = new[]
                        {
                            (byte)(retBuffers[1] & 0xff), (byte)((retBuffers[1] >> 8) & 0xff),
                            (byte)(retBuffers[0] & 0xff), (byte)((retBuffers[0] >> 8) & 0xff)
                        };
                        ret.Value = BitConverter.ToSingle(bytes, 0);
                    }
                    else if (ioarg.ValueType.ToString().Contains("AsciiString"))
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

        private ushort GetModbusReadCount(uint functionCode, DataTypeEnum dataType)
        {
            if (dataType.ToString().Contains("32") || dataType.ToString().Contains("Float"))
                return 2;
            if (dataType.ToString().Contains("64") || dataType.ToString().Contains("Double"))
                return 4;
            return 1;
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

        private DriverReturnValueModel AnalyzeAddress(DriverAddressIoArgModel ioarg, out ushort startAddress,
            out ushort readCount)
        {
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            try
            {
                if (ioarg.ValueType == DataTypeEnum.AsciiString)
                {
                    startAddress = ushort.Parse(ioarg.Address.Split(',')[0]);
                    readCount = ushort.Parse(ioarg.Address.Split(',')[1]);
                }
                else
                {
                    startAddress = ushort.Parse(ioarg.Address);
                    readCount = GetModbusReadCount(3, ioarg.ValueType);
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

        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false };
            try
            {
                if (!IsConnected)
                    rpcResponse.Description = "设备连接已断开";
                else
                {
                    ushort address, count;
                    AnalyzeAddress(ioarg, out address, out count);

                    //功能码01
                    if (method == nameof(ReadCoil))
                    {
                        var value = ioarg.Value.ToString() == "1" || ioarg.Value.ToString().ToLower() == "true";
                        await _master.WriteSingleCoilAsync(SlaveAddress, address, value);
                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }

                    //功能码03
                    if (method == nameof(ReadHoldingRegisters))
                    {
                        ushort[] shortArray = new ushort[count];

                        switch (ioarg.ValueType)
                        {
                            case DataTypeEnum.AsciiString:
                                ModbusDataConvert.SetString(shortArray, 0, ioarg.Value.ToString());
                                await _master.WriteMultipleRegistersAsync(SlaveAddress, address, shortArray);

                                break;
                            case DataTypeEnum.Float:
                                float f = 0;
                                float.TryParse(ioarg.Value.ToString(), out f);
                                ModbusDataConvert.SetReal(shortArray, 0, f);
                                await _master.WriteMultipleRegistersAsync(SlaveAddress, address, shortArray);

                                break;
                            default:
                                await _master.WriteSingleRegisterAsync(SlaveAddress, address,
                                    ushort.Parse(ioarg.Value.ToString()));
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
                rpcResponse.Description = $"写入失败,[method]:{method},[ioarg]:{ioarg},[ex]:{ex}";
                _logger.LogError(ex, $"Device:[{_device}],WriteAsync(),Error");
            }

            return rpcResponse;
        }
    }
}