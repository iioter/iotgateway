using SimpleTCP;
using System.Text;
using System.IO.Ports;
using PluginInterface;
using Microsoft.Extensions.Logging;

namespace Other.Toledo
{
    [DriverSupported("Toledo")]
    [DriverInfo("Toledo", "V1.0.0", "Copyright IoTGateway.net 20230220")]
    public class DeviceToledo : IDriver
    {
        /// <summary>
        /// tcp客户端
        /// </summary>
        private SimpleTcpClient? client;
        /// <summary>
        /// 缓存最新的服务器返回的原始数据
        /// </summary>
        private byte[]? latestRcvData;

        private byte[] TempRcvData;
        private DateTime latestDate;

        public ILogger _logger { get; set; }
        private readonly string _device;
        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 6666;

        [ConfigParameter("数据位")]
        public int DataBits { get; set; } = 17;

        [ConfigParameter("校验位")]
        public Parity Parity { get; set; } = Parity.None;

        [ConfigParameter("停止位")]
        public StopBits StopBits { get; set; } = StopBits.One;

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

        #region 生命周期

        /// <summary>
        /// 反射构造函数
        /// </summary>
        /// <param name="device"></param>
        /// <param name="logger"></param>
        public DeviceToledo(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{_device}],Create()");
        }


        /// <summary>
        /// 判断连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                //客户端对象不为空并且客户端已连接则返回true
                return client != null && client.TcpClient != null && client.TcpClient.Connected;
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
                _logger.LogInformation($"Device:[{_device}],Connect()");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Connect()");
                return false;
            }
            return IsConnected;
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

                _logger.LogInformation($"Device:[{_device}],Close()");
                return !IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Close()");

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

                // Suppress finalization.
                GC.SuppressFinalize(this);
                _logger.LogInformation($"Device:[{_device}],Dispose()");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Dispose()");

            }
        }

        #endregion

        #region 读写方法
        /// <summary>
        /// 解析并返回
        /// </summary>
        /// <param name="ioArg">ioArg.Address为起始变量字节编号;ioArg.ValueType为类型</param>
        /// <returns></returns>
        [Method("读模拟设备数据", description: "读模拟设备数据,开始字节和长度")]
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            //连接正常则进行读取
            if (IsConnected)
            {
                try
                {
                    ushort address, count;
                    ret = AnalyseAddress(ioArg, out address, out count);



                    if (
                        (latestRcvData == null) ||
                        (DateTime.Now.Subtract(latestDate).TotalSeconds > 10)
                        )
                    {
                        latestRcvData = null;
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = "没有收到数据";

                        if (ioArg.ValueType == DataTypeEnum.Custome1)
                        {
                            ret.Value = latestDate;
                            ret.StatusType = VaribaleStatusTypeEnum.Good;
                        }
                    }
                    else
                    {

                        //解析数据，并返回
                        switch (ioArg.ValueType)
                        {
                            case DataTypeEnum.UByte:

                            case DataTypeEnum.Byte:
                                ret.Value = latestRcvData[address];
                                break;
                            case DataTypeEnum.Int16:
                                ret.Value = (short)latestRcvData[address];
                                //var buffer16 = latestRcvData.Skip(address).Take(count).ToArray();
                                //ret.Value = BitConverter.ToInt16(buffer16[0], 0);
                                break;
                            case DataTypeEnum.Int32:
                                //拿到有用的数据
                                var bufferCustome1 = latestRcvData.Skip(address).Take(count).ToArray();

                                var strCustome1 = Encoding.ASCII.GetString(bufferCustome1);
                                if (strCustome1.Contains("\0"))
                                    strCustome1 = strCustome1.Replace("\0", "");

                                ret.Value = strCustome1 == "" ? 0 : int.Parse(strCustome1);

                                break;
                            case DataTypeEnum.Float:
                                //拿到有用的数据
                                var buffer32 = latestRcvData.Skip(address).Take(count).ToArray();
                                //大小端转换一下
                                ret.Value = BitConverter.ToSingle(buffer32, 0);
                                break;
                            case DataTypeEnum.AsciiString:
                                //拿到有用的数据
                                var bufferAscii = latestRcvData.Skip(address).Take(count).ToArray();


                                var str = Encoding.ASCII.GetString(bufferAscii);
                                if (str.Contains("\0"))
                                    str = str.Replace("\0", "");

                                ret.Value = str;
                                break;
                            case DataTypeEnum.Custome1:
                                ret.Value = latestDate;
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


        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioArg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            await Task.CompletedTask;
            return rpcResponse;
        }
        #endregion

        #region 私有方法

        private byte[] addBytes(byte[] data1, byte[] data2)
        {
            byte[] data3 = new byte[(data1 != null ? data1.Length : 0) + data2.Length]; ;

            if (data1 != null)
            {
                data1.CopyTo(data3, 0);
            }
            if (data2 != null)
                data2.CopyTo(data3, data1 != null ? data1.Length : 0);

            return data3;
        }

        /// <summary>
        /// 收到服务端数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_DataReceived(object? sender, Message e)
        {
            if (e.Data.Length == 0) { return; }

            latestDate = DateTime.Now;


            //合并TempRcvData, e.Data
            byte[] RcvData = addBytes(TempRcvData, e.Data);

            //_logger.LogInformation($"Device:[{_device}],TempRcvData:{BitConverter.ToString(TempRcvData)}," +
            //    $"e.Data:{BitConverter.ToString(e.Data)},RcvData:{BitConverter.ToString(RcvData)}");

            int index = RcvData.Length - 1;

            if (RcvData.Length >= DataBits)
            {
                while (index > 0)
                {
                    if (RcvData[index] != 0x0d)
                    {
                        index--;
                        continue;
                    }
                    else
                    {
                        if (index + 1 >= DataBits && RcvData[index - DataBits + 1] == 0x02)
                        {
                            latestRcvData = new byte[DataBits];
                            Array.Copy(RcvData, index - DataBits + 1, latestRcvData, 0, DataBits);


                            if (RcvData.Length - index > 1)
                            {
                                TempRcvData = new byte[RcvData.Length - index];
                                Array.Copy(RcvData, index + 1, TempRcvData, 0, RcvData.Length - index - 1);
                            }


                            return;
                        }
                        else
                        {
                            if (latestRcvData != null) { Array.Clear(latestRcvData, 0, latestRcvData.Length); }
                        }
                        break;
                    }
                }

                if (TempRcvData != null)
                {
                    Array.Clear(TempRcvData, 0, TempRcvData.Length);
                }
            }
            else
            {
                TempRcvData = RcvData;
            }
        }


        /// <summary>
        /// 分析地址的开始地址和总位数
        /// </summary>
        /// <param name="ioarg"></param>
        /// <param name="StartAddress">起始位置</param>
        /// <param name="ReadCount">总位数</param>
        /// <returns></returns>
        private DriverReturnValueModel AnalyseAddress(DriverAddressIoArgModel ioarg, out ushort StartAddress, out ushort ReadCount)
        {
            DriverReturnValueModel ret = new() { StatusType = VaribaleStatusTypeEnum.Good };
            try
            {
                if (ioarg.ValueType == DataTypeEnum.AsciiString)
                {
                    StartAddress = ushort.Parse(ioarg.Address.Split(',')[0]);
                    ReadCount = ushort.Parse(ioarg.Address.Split(',')[1]);
                }
                else
                {
                    StartAddress = ushort.Parse(ioarg.Address);
                    ReadCount = 1;
                }
                return ret;

            }
            catch (Exception ex)
            {
                ret.StatusType = VaribaleStatusTypeEnum.AddressError;
                ret.Message = ex.Message;
                StartAddress = 0;
                ReadCount = 0;
                return ret;
            }
        }

        #endregion
    }

    public enum ConnectionType
    {
        Long,
        Short
    }
}