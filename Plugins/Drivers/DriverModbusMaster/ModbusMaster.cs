using Modbus.Device;
using Modbus.Serial;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;

namespace DriverModbusMaster
{
    [DriverSupported("ModbusTCP")]
    [DriverSupported("ModbusUDP")]
    [DriverSupported("ModbusRtu")]
    [DriverSupported("ModbusAscii")]
    [DriverInfoAttribute("ModbusMaster", "V1.0.0", "Copyright WHD© 2021-12-19")]
    public class ModbusMaster : IDriver
    {
        private TcpClient clientTcp = null;
        private UdpClient clientUdp = null;
        private SerialPort port = null;
        private Modbus.Device.ModbusMaster master = null;
        private SerialPortAdapter adapter = null;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("PLC类型")]
        public PLC_TYPE PLCType { get; set; } = PLC_TYPE.S71200;

        [ConfigParameter("主站类型")]
        public Master_TYPE Master_TYPE { get; set; } = Master_TYPE.Tcp;

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 502;

        [ConfigParameter("串口名")]
        public string PortName { get; set; } = "COM1";

        [ConfigParameter("波特率")]
        public int BaudRate { get; set; } = 9600;

        [ConfigParameter("数据位")]
        public int DataBits { get; set; } = 8;

        [ConfigParameter("校验位")]
        public Parity Parity { get; set; } = Parity.None;

        [ConfigParameter("停止位")]
        public StopBits StopBits { get; set; } = StopBits.One;

        [ConfigParameter("从站号")]
        public byte SlaveAddress { get; set; } = 1;

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public ModbusMaster(Guid deviceId)
        {
            DeviceId = deviceId;
        }


        public bool IsConnected
        {
            get
            {
                switch (Master_TYPE)
                {
                    case Master_TYPE.Tcp:
                    case Master_TYPE.RtuOnTcp:
                    case Master_TYPE.AsciiOnTcp:
                        return clientTcp != null && master != null && clientTcp.Connected;
                    case Master_TYPE.Udp:
                    case Master_TYPE.RtuOnUdp:
                    case Master_TYPE.AsciiOnUdp:
                        return clientUdp != null && master != null && clientUdp.Client.Connected;
                    case Master_TYPE.Rtu:
                    case Master_TYPE.Ascii:
                        return port != null && master != null && port.IsOpen;
                    default:
                        return false;
                }
            }
        }

        public bool Connect()
        {
            try
            {
                switch (Master_TYPE)
                {
                    case Master_TYPE.Tcp:
                        clientTcp = new TcpClient(IpAddress.ToString(), Port);
                        clientTcp.ReceiveTimeout = Timeout;
                        clientTcp.SendTimeout = Timeout;
                        master = ModbusIpMaster.CreateIp(clientTcp);
                        break;
                    case Master_TYPE.Udp:
                        clientUdp = new UdpClient(IpAddress.ToString(), Port);
                        clientUdp.Client.ReceiveTimeout = Timeout;
                        clientUdp.Client.SendTimeout = Timeout;
                        master = ModbusIpMaster.CreateIp(clientUdp);
                        break;
                    case Master_TYPE.Rtu:
                        port = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                        port.ReadTimeout = Timeout;
                        port.WriteTimeout = Timeout;
                        port.Open();
                        adapter = new SerialPortAdapter(port);
                        master = ModbusSerialMaster.CreateRtu(adapter);
                        break;
                    case Master_TYPE.RtuOnTcp:
                        clientTcp = new TcpClient(IpAddress.ToString(), Port);
                        clientTcp.ReceiveTimeout = Timeout;
                        clientTcp.SendTimeout = Timeout;
                        master = ModbusSerialMaster.CreateRtu(clientTcp);
                        break;
                    case Master_TYPE.RtuOnUdp:
                        clientUdp = new UdpClient(IpAddress.ToString(), Port);
                        clientUdp.Client.ReceiveTimeout = Timeout;
                        clientUdp.Client.SendTimeout = Timeout;
                        master = ModbusSerialMaster.CreateRtu(clientUdp);
                        break;
                    case Master_TYPE.Ascii:
                        port = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                        port.ReadTimeout = Timeout;
                        port.WriteTimeout = Timeout;
                        port.Open();
                        adapter = new SerialPortAdapter(port);
                        master = ModbusSerialMaster.CreateAscii(adapter);
                        break;
                    case Master_TYPE.AsciiOnTcp:
                        clientTcp = new TcpClient(IpAddress.ToString(), Port);
                        clientTcp.ReceiveTimeout = Timeout;
                        clientTcp.SendTimeout = Timeout;
                        master = ModbusSerialMaster.CreateAscii(clientTcp);
                        break;
                    case Master_TYPE.AsciiOnUdp:
                        clientUdp = new UdpClient(IpAddress.ToString(), Port);
                        clientUdp.Client.ReceiveTimeout = Timeout;
                        clientUdp.Client.SendTimeout = Timeout;
                        master = ModbusSerialMaster.CreateAscii(clientUdp);
                        break;
                    default:
                        break;
                }
                master.Transport.ReadTimeout = Timeout;
                master.Transport.WriteTimeout = Timeout;
            }
            catch (Exception ex)
            {
                return false;
            }
            return IsConnected;
        }

        public bool Close()
        {
            try
            {
                clientTcp?.Close();
                clientUdp?.Close();
                port?.Close();
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
                clientTcp?.Dispose();
                clientUdp?.Dispose();
                port?.Dispose();
                master?.Dispose();
            }
            catch (Exception)
            {

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
                    var retBool = master.ReadCoils(SlaveAddress, ushort.Parse(ioarg.Address), 1)[0];
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
                    var retBool = master.ReadInputs(SlaveAddress, ushort.Parse(ioarg.Address), 1)[0];
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
        private DriverReturnValueModel ReadRegistersBuffers(byte FunCode, DriverAddressIoArgModel ioarg)
        {
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            if (!IsConnected)
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
            else
            {
                ushort startAddress;
                if (!ushort.TryParse(ioarg.Address.Trim(), out startAddress))
                    ret.StatusType = VaribaleStatusTypeEnum.AddressError;
                else
                {
                    var count = GetModbusReadCount(3, ioarg.ValueType);
                    try
                    {
                        var rawBuffers = new ushort[] { };
                        if (FunCode == 3)
                            rawBuffers = master.ReadHoldingRegisters(SlaveAddress, startAddress, count);
                        else if (FunCode == 4)
                            rawBuffers = master.ReadInputRegisters(SlaveAddress, startAddress, count);

                        var retBuffers = ChangeBuffersOrder(rawBuffers, ioarg.ValueType);
                        if (ioarg.ValueType.ToString().Contains("Uint16"))
                            ret.Value = retBuffers[0];
                        else if (ioarg.ValueType.ToString().Contains("Int16"))
                            ret.Value = (short)retBuffers[0];
                        else if (ioarg.ValueType.ToString().Contains("Uint32"))
                            ret.Value = (UInt32)(retBuffers[0] << 16) + retBuffers[1];
                        else if (ioarg.ValueType.ToString().Contains("Int32"))
                            ret.Value = (Int32)(retBuffers[0] << 16) + retBuffers[1];
                        else if (ioarg.ValueType.ToString().Contains("Float"))
                        {
                            var bytes = new byte[] { (byte)(retBuffers[1] & 0xff), (byte)((retBuffers[1] >> 8) & 0xff), (byte)(retBuffers[0] & 0xff), (byte)((retBuffers[0] >> 8) & 0xff) };
                            ret.Value = BitConverter.ToSingle(bytes, 0);
                        }

                    }
                    catch (Exception ex)
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = ex.Message;
                    }
                }
            }

            return ret;
        }

        private ushort GetModbusReadCount(uint functionCode, DataTypeEnum dataType)
        {
            if (dataType.ToString().Contains("32") || dataType.ToString().Contains("Float"))
                return 2;
            else if (dataType.ToString().Contains("64") || dataType.ToString().Contains("Double"))
                return 4;
            else
                return 1;
        }

        //预留了大小端转换的 
        private ushort[] ChangeBuffersOrder(ushort[] buffers, DataTypeEnum dataType)
        {
            var newBuffers = new ushort[buffers.Length];
            if (dataType.ToString().Contains("32") || dataType.ToString().Contains("Float"))
            {
                var A = buffers[0] & 0xff00;//A
                var B = buffers[0] & 0x00ff;//B
                var C = buffers[1] & 0xff00;//C
                var D = buffers[1] & 0x00ff;//D
                if (dataType.ToString().Contains("_1"))
                {
                    newBuffers[0] = (ushort)(A + B);//AB
                    newBuffers[1] = (ushort)(C + D);//CD
                }
                else if (dataType.ToString().Contains("_2"))
                {
                    newBuffers[0] = (ushort)((A >> 8) + (B << 8));//BA
                    newBuffers[1] = (ushort)((C >> 8) + (D << 8));//DC
                }
                else if (dataType.ToString().Contains("_3"))
                {
                    newBuffers[0] = (ushort)((C >> 8) + (D << 8));//DC
                    newBuffers[1] = (ushort)((A >> 8) + (B << 8));//BA
                }
                else
                {
                    newBuffers[0] = (ushort)(C + D);//CD
                    newBuffers[1] = (ushort)(A + B);//AB
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

        public async Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel Ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false };
            try
            {
                ushort address = ushort.Parse(Ioarg.Address);
                if (!IsConnected)
                    rpcResponse.Description = "设备连接已断开";
                else
                {
                    //功能码01
                    if (Method == nameof(ReadCoil))
                    {
                        bool value = Ioarg.Value.ToString() == "1" || Ioarg.Value.ToString().ToLower() == "true";
                        master.WriteSingleCoilAsync(SlaveAddress, address, value);
                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }
                    //功能码03
                    else if (Method == nameof(ReadHoldingRegisters))
                    {
                        master.WriteSingleRegisterAsync(SlaveAddress, address, ushort.Parse(Ioarg.Value.ToString()));
                        rpcResponse.IsSuccess = true;
                        return rpcResponse;
                    }
                    else
                        rpcResponse.Description = $"不支持写入:{Method}";
                }
            }
            catch (Exception ex)
            {
                rpcResponse.Description = $"写入失败:{Method},{Ioarg}";
            }
            return rpcResponse;
        }
    }
    public enum PLC_TYPE
    {
        S7200 = 0,
        S7300 = 1,
        S7400 = 2,
        S71200 = 3,
        S71500 = 4,
    }

    public enum Master_TYPE
    {
        Tcp = 0,
        Udp = 1,
        Rtu = 2,
        RtuOnTcp = 3,
        RtuOnUdp = 4,
        Ascii = 5,
        AsciiOnTcp = 6,
        AsciiOnUdp = 7,
    }
}
