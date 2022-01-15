using PluginInterface;
using System;
using OpenNETCF.MTConnect;
namespace DriverMTConnect
{
    internal class MTConnectClient : IDriver
    {

        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("uri")]
        public string Uri { get; set; }

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        public bool IsConnected { get; set; }

        #endregion

        EntityClient m_client = null;

        public MTConnectClient(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public bool Close()
        {
            try
            {
                m_client = null;
                IsConnected = false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Connect()
        {
            try
            {
                m_client = new EntityClient(Uri);
                m_client.RequestTimeout = Timeout;
                IsConnected = true;
            }
            catch (Exception)
            {

                IsConnected = false;
            }
            return IsConnected;
        }

        public void Dispose()
        {
            return;
        }

        [Method("读MTConnect", description: "读MTConnect ID")]
        public DriverReturnValueModel ReadById(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    var dataValue = m_client.GetDataItemById(Ioarg.Address).Value;
                    ret.Value = dataValue;
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

        public DriverReturnValueModel Read(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            return ret;
        }
    }

}