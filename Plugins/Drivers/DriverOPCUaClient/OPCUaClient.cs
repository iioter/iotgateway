using PluginInterface;
using System;
using System.Collections;
using System.IO;
using Opc.Ua;
using Opc.Ua.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Opc.Ua.Configuration;
using OpcUaHelper;

namespace DriverOPCUaClient
{
    [DriverSupported("OPC UA")]
    [DriverInfoAttribute("OPCUaClient", "V1.0.0", "Copyright IoTGateway© 2021-12-19")]
    public class OPCUaClient : IDriver
    {
        OpcUaClientHelper opcUaClient = null;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("uri")]
        public string Uri { get; set; } = "opc.tcp://localhost:62541/Quickstarts/ReferenceServer";

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public OPCUaClient(Guid deviceId)
        {
            DeviceId = deviceId;

        }


        public bool IsConnected
        {
            get
            {

                return opcUaClient != null && opcUaClient.Connected;
            }
        }

        public bool Connect()
        {
            try
            {
                opcUaClient = new OpcUaClientHelper();
                opcUaClient.ConnectServer(Uri).Wait(Timeout);
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
                opcUaClient?.Disconnect();
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
                opcUaClient = null;
            }
            catch (Exception)
            {

            }
        }


        [Method("读OPCUa", description: "读OPCUa节点")]
        public DriverReturnValueModel ReadNode(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    var dataValue = opcUaClient.ReadNode(new NodeId(ioarg.Address));
                    if (DataValue.IsGood(dataValue))
                        ret.Value = dataValue.Value;
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
