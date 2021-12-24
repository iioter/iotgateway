using PluginInterface;
using System;
using System.Collections;
using System.IO;
using Opc.Ua;
using Opc.Ua.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Opc.Ua.Configuration;

namespace DriverOPCUaClient
{
    [DriverSupported("OPC UA")]
    [DriverInfoAttribute("OPCUaClient", "V1.0.0", "Copyright WHD© 2021-12-19")]
    public class OPCUaClient : IDriver
    {
        Session session = null;
        ApplicationConfiguration config = null;
        ConfiguredEndpoint endpoint = null;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("uri")]
        public string Uri { get; set; } = "opc.tcp://localhost:62541/Quickstarts/ReferenceServer";

        [ConfigParameter("超时时间ms")]
        public uint Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public OPCUaClient(Guid deviceId)
        {
            DeviceId = deviceId;

            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = "ConsoleReferenceClient",
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "Quickstarts.ReferenceClient",
                CertificatePasswordProvider = new CertificatePasswordProvider(null)
            };
            config = application.LoadApplicationConfiguration(silent: false).Result;

            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(application.ApplicationConfiguration, Uri, false);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(application.ApplicationConfiguration);
            endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
        }


        public bool IsConnected
        {
            get
            {

                return session != null && session.Connected;
            }
        }

        public bool Connect()
        {
            try
            {
                session = Session.Create(config, endpoint, false, false, config.ApplicationName, 30 * 60 * 1000, new UserIdentity(), null).Result;
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
                session?.Close();
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
                session?.Dispose();
                session = null;
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
                    var dataValue = session.ReadValue(new NodeId(ioarg.Address));
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
    }
}
