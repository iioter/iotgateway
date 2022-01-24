using IoTClient.Clients.PLC;
using IoTClient.Enums;
using PluginInterface;
using System;
using System.Text;

namespace DriverMitsubishi
{
    [DriverSupported("A_1E")]
    [DriverSupported("Qna_3E")]
    [DriverInfoAttribute("Mitsubishi", "V1.0.0", "Copyright WHD-IoTClient© 2022-01-15")]
    public class Mitsubishi : IDriver
    {
        private MitsubishiClient plc = null;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("PLC类型")]
        public MitsubishiVersion CpuType { get; set; } = MitsubishiVersion.Qna_3E;

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 6000;

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public Mitsubishi(Guid deviceId)
        {
            DeviceId = deviceId;
        }


        public bool IsConnected
        {
            get
            {
                return plc != null && plc.Connected;
            }
        }

        public bool Connect()
        {
            try
            {
                plc = new MitsubishiClient(MitsubishiVersion.Qna_3E, IpAddress, Port);
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

            if (plc != null && this.IsConnected)
            {
                try
                {
                    switch (ioarg.ValueType)
                    {
                        case PluginInterface.DataTypeEnum.Bit:
                            ret.Value = plc.ReadBoolean(ioarg.Address).Value == true ? 1 : 0;
                            break;
                        case PluginInterface.DataTypeEnum.Bool:
                            ret.Value = plc.ReadBoolean(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.UByte:
                            ret.Value = plc.ReadByte(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Byte:
                            ret.Value = (sbyte)plc.ReadByte(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint16:
                            ret.Value =plc.ReadUInt16(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int16:
                            ret.Value = plc.ReadInt16(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint32:
                            ret.Value =plc.ReadUInt32(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int32:
                            ret.Value = plc.ReadInt32(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Float:
                            ret.Value = plc.ReadFloat(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Double:
                            ret.Value = plc.ReadDouble(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Uint64:
                            ret.Value = plc.ReadUInt64(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.Int64:
                            ret.Value = plc.ReadInt64(ioarg.Address).Value;
                            break;
                        case PluginInterface.DataTypeEnum.AsciiString:
                            ret.Value = plc.ReadString(ioarg.Address);
                            break;
                        case PluginInterface.DataTypeEnum.Utf8String:
                            ret.Value = plc.ReadString(ioarg.Address);
                            break;
                        default:
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

    }
}