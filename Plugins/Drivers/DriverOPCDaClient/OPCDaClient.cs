using PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.OPCClient;

namespace DriverOPCDaClient
{
    internal class OPCDaClient : IDriver
    {
        OPCClientWrapper opcDaClient = null;

        #region 配置参数

        [ConfigParameter("设备Id")] public Guid DeviceId { get; set; }

        [ConfigParameter("IP")]
        public string IP{ get; set; } = "127.0.0.1";

        [ConfigParameter("OpcServerName")]
        public string OpcServerName { get; set; } = "ICONICS.SimulatorOPCDA.2";

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        public OPCDaClient(Guid deviceId)
        {
            DeviceId = deviceId;

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
                opcDaClient.Init(IP, OpcServerName);
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
                    var dataValue = opcDaClient.ReadNodeLabel(ioarg.Address);
                    switch (ioarg.ValueType)
                    {
                        case DataTypeEnum.Bit:
                            ret.Value = dataValue == "On" ? 1 : 0;
                            break;
                        case DataTypeEnum.Bool:
                            ret.Value = dataValue == "On";
                            break;
                        case DataTypeEnum.Byte:
                            ret.Value = byte.Parse(dataValue);
                            break;
                        case DataTypeEnum.UByte:
                            ret.Value = sbyte.Parse(dataValue);
                            break;
                        case DataTypeEnum.Int16:
                            ret.Value = short.Parse(dataValue);
                            break;
                        case DataTypeEnum.Uint16:
                            ret.Value = ushort.Parse(dataValue);
                            break;
                        case DataTypeEnum.Int32:
                            ret.Value = int.Parse(dataValue);
                            break;
                        case DataTypeEnum.Uint32:
                            ret.Value = uint.Parse(dataValue);
                            break;
                        case DataTypeEnum.Float:
                            ret.Value = float.Parse(dataValue);
                            break;
                        case DataTypeEnum.Double:
                            ret.Value = double.Parse(dataValue);
                            break;
                        case DataTypeEnum.AsciiString:
                        case DataTypeEnum.Utf8String:
                            ret.Value = dataValue;
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

        public async Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel Ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return rpcResponse;
        }
    }
}
