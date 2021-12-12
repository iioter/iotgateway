using Modbus.Device;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DriverModbusTCP
{
    [DriverSupported("ModbusTCP")]
    [DriverSupported("ModbusUDP")]
    [DriverInfoAttribute("ModbusTCP", "V1.0.0", "Copyright WHD© 2021-12-19")]
    public class ModbusTCP : IDriver
    {
        private TcpClient client = null;
        private ModbusIpMaster master = null;

        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("PLC类型")]
        public PLC_TYPE PLCType { get; set; } = PLC_TYPE.S71200;

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 502;

        [ConfigParameter("站号")]
        public byte SlaveId { get; set; } = 1;

        [ConfigParameter("超时时间ms")]
        public uint Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 30000;

        #endregion

        public ModbusTCP(Guid deviceId)
        {
            DeviceId = deviceId;
        }


        public bool IsConnected
        {
            get
            {
                return client != null && master != null && client.Connected;
            }
        }

        public bool Connect()
        {
            try
            {
                client = new TcpClient(IpAddress.ToString(), Port);
                master = ModbusIpMaster.CreateIp(client);
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
                client?.Close();
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
                client?.Dispose();
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
            if (!client.Connected)
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
                            rawBuffers = master.ReadHoldingRegisters(SlaveId, startAddress, count);
                        else if (FunCode == 4)
                            rawBuffers = master.ReadHoldingRegisters(SlaveId, startAddress, count);

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
    }
    public enum PLC_TYPE
    {
        S7200 = 0,
        S7300 = 1,
        S7400 = 2,
        S71200 = 3,
        S71500 = 4,
    }
}
