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
                            var resultBit = plc.ReadBoolean(ioarg.Address);
                            if (resultBit.IsSucceed)
                                ret.Value = resultBit.Value == true ? 1 : 0;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Bool:
                            var resultBool = plc.ReadBoolean(ioarg.Address);
                            if (resultBool.IsSucceed)
                                ret.Value = resultBool.Value == true ? 1 : 0;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.UByte:
                            var resultUByte = plc.ReadByte(ioarg.Address);
                            if (resultUByte.IsSucceed)
                                ret.Value = resultUByte.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Byte:
                            var resultByte = plc.ReadByte(ioarg.Address);
                            if (resultByte.IsSucceed)
                                ret.Value = (sbyte)resultByte.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Uint16:
                            var resultUint = plc.ReadUInt16(ioarg.Address);
                            if (resultUint.IsSucceed)
                                ret.Value = resultUint.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Int16:
                            var resultInt = plc.ReadInt16(ioarg.Address);
                            if (resultInt.IsSucceed)
                                ret.Value = resultInt.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Uint32:
                            var resultUint32 = plc.ReadUInt32(ioarg.Address);
                            if (resultUint32.IsSucceed)
                                ret.Value = resultUint32.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Int32:
                            var resultInt32 = plc.ReadInt32(ioarg.Address);
                            if (resultInt32.IsSucceed)
                                ret.Value = resultInt32.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Float:
                            var resultFloat = plc.ReadFloat(ioarg.Address);
                            if (resultFloat.IsSucceed)
                                ret.Value = resultFloat.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Double:
                            var resultDouble = plc.ReadDouble(ioarg.Address);
                            if (resultDouble.IsSucceed)
                                ret.Value = resultDouble.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Uint64:
                            var resultUint64 = plc.ReadUInt64(ioarg.Address);
                            if (resultUint64.IsSucceed)
                                ret.Value = resultUint64.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Int64:
                            var resultInt64 = plc.ReadInt64(ioarg.Address);
                            if (resultInt64.IsSucceed)
                                ret.Value = resultInt64.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.AsciiString:
                            var resultIntAsciiStr = plc.ReadString(ioarg.Address);
                            if (resultIntAsciiStr.IsSucceed)
                                ret.Value = resultIntAsciiStr.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            break;
                        case PluginInterface.DataTypeEnum.Utf8String:
                            var resultIntUtf8Str = plc.ReadString(ioarg.Address);
                            if (resultIntUtf8Str.IsSucceed)
                                ret.Value = resultIntUtf8Str.Value;
                            else
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
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