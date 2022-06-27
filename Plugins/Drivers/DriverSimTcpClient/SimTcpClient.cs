using PluginInterface;
using SimpleTCP;
using System;
using System.Text;

namespace DriverSimTcpClient
{
    [DriverSupported("SimTcpServerDevice")]
    [DriverInfoAttribute("SimTcpClient", "V1.0.0", "Copyright IoTGateway© 2022-06-04")]
    public class SimTcpClient : IDriver
    {
        /// <summary>
        /// tcp客户端
        /// </summary>
        private SimpleTcpClient? client;
        /// <summary>
        /// 缓存最新的服务器返回的原始数据
        /// </summary>
        private byte[] latestRcvData;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 6666;

        /// <summary>
        /// 为了演示枚举类型在web端的录入，这里没用到 但是你可以拿到
        /// </summary>
        [ConfigParameter("连接类型")]
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Long;

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 300;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public SimTcpClient(Guid deviceId)
        {
            DeviceId = deviceId;
        }


        /// <summary>
        /// 判断连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                //客户端对象不为空并且客户端已连接则返回true
                return client != null && client.TcpClient.Connected;
            }
        }

        /// <summary>
        /// 进行连接
        /// </summary>
        /// <returns>连接是否成功</returns>
        public bool Connect()
        {
            try
            {
                //进行连接
                client = new SimpleTcpClient().Connect(IpAddress, Port);
                client.DataReceived += Client_DataReceived;
            }
            catch (Exception)
            {
                return false;
            }
            return IsConnected;
        }
        /// <summary>
        /// 收到服务端数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DataReceived(object? sender, Message e)
        {
            //如果收到的数据校验正确，则放在内存中
            if (e.Data.Length == 8 && e.Data[0] == 0x08)
                latestRcvData = e.Data;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>断开是否成功</returns>
        public bool Close()
        {
            try
            {
                client.DataReceived -= Client_DataReceived;
                //断开连接
                client?.Disconnect();
                return !IsConnected;
            }
            catch (Exception)
            {

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
                //释放资源
                client?.Dispose();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private byte[] sendCmd = new byte[4] { 0x01, 0x02, 0x03, 0x04 };

        /// <summary>
        /// 解析并返回
        /// </summary>
        /// <param name="ioarg">ioarg.Address为起始变量字节编号;ioarg.ValueType为类型</param>
        /// <returns></returns>
        [Method("读模拟设备数据", description: "读模拟设备数据,开始字节和长度")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            ushort startIndex;
            //判断地址是否为整数
            if (!ushort.TryParse(ioarg.Address, out startIndex))
            {
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                ret.Message = "起始字节编号错误";
                return ret;
            }
            //连接正常则进行读取
            if (IsConnected)
            {
                try
                {
                    //发送请求
                    client?.Write(sendCmd);
                    //等待恢复，这里可以优化
                    Thread.Sleep(Timeout);
                    if (latestRcvData == null)
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = "没有收到数据";
                    }
                    else
                    {
                        //解析数据，并返回
                        switch (ioarg.ValueType)
                        {
                            case DataTypeEnum.UByte:
                            case DataTypeEnum.Byte:
                                ret.Value = latestRcvData[startIndex];
                                break;
                            case DataTypeEnum.Int16:
                                var buffer16 = latestRcvData.Skip(startIndex).Take(2).ToArray();
                                ret.Value = BitConverter.ToInt16(new byte[] { buffer16[0], buffer16[1] }, 0);
                                break;
                            case DataTypeEnum.Float:
                                //拿到有用的数据
                                var buffer32 = latestRcvData.Skip(startIndex).Take(4).ToArray();
                                //大小端转换一下
                                ret.Value = BitConverter.ToSingle(new byte[] { buffer32[3], buffer32[2], buffer32[1], buffer32[0] }, 0);
                                break;
                            default:
                                break;
                        }
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


        public async Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel Ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return rpcResponse;
        }
    }

    public enum ConnectionType
    {
        Long,
        Short
    }
}