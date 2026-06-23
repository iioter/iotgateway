using PluginInterface;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace CNC.Fanuc
{
    [DriverSupported("Fanuc")]
    [DriverInfo("Fanuc", "V1.0.0", "Copyright iotgateway.net 20230220")]
    public class DeviceFanuc : IDriver
    {
        private ushort _hndl;
        private int _result = -1;

        public ILogger _logger { get; set; }
        private readonly string _device;
        public event Func<object, DataReportEventArgs, Task>? OnDataReceived;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }
        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;
        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;
        [ConfigParameter("设备Ip")] public string DeviceIp { get; set; } = "127.0.0.1";
        [ConfigParameter("设备Port")] public int DevicePort { get; set; } = 8193;

        #endregion

        #region 生命周期
        public DeviceFanuc(string device, ILogger logger)
        {
            _device = device;
            _logger = logger;

            _logger.LogInformation($"Device:[{device}],Create()");
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    Focas1.ODBSYS a = new Focas1.ODBSYS();
                    _result = FanucFocas.cnc_sysinfo(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                        return true;
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Connect()
        {
            try
            {
                _result = -1;
                _result = FanucFocas.cnc_allclibhndl3(DeviceIp, Convert.ToUInt16(DevicePort), Convert.ToInt32(Timeout),
                    out _hndl);

                if (_result == FanucFocas.EW_OK)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                _result = FanucFocas.cnc_freelibhndl(_hndl);
                if (_result == FanucFocas.EW_OK)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 读写方法


        [Method("Fanuc", description: "读Fanuc设备类型")]
        public DriverReturnValueModel ReadDeviceType(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBSYS a = new Focas1.ODBSYS();
                    _result = FanucFocas.cnc_sysinfo(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        string type = new string(a.mt_type);
                        string num = new string(a.cnc_type);
                        ret.Value = type + num;
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

        [Method("Fanuc", description: "读Fanuc设备运行状态")]
        public DriverReturnValueModel ReadDeviceRunStatus(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBSYS a = new Focas1.ODBSYS();
                    _result = FanucFocas.cnc_sysinfo(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        string type = new string(a.mt_type);
                        string num = new string(a.cnc_type);
                        Focas1.ODBST aa = new Focas1.ODBST();
                        _result = FanucFocas.cnc_statinfo(_hndl, aa);
                        if (_result == FanucFocas.EW_OK)
                        {
                            var strRet = string.Empty;
                            var runRet = aa.run;
                            if (num == "15")
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "STOP";
                                        break;
                                    case 1:
                                        strRet = "HOLD";
                                        break;
                                    case 2:
                                        strRet = "START";
                                        break;
                                    case 3:
                                        strRet = "MSTR(jog mdi)";
                                        break;
                                    case 4:
                                        strRet = "ReSTART(not blinking)";
                                        break;
                                    case 5:
                                        strRet = "PRSR(program restart)";
                                        break;
                                    case 6:
                                        strRet = "NSRC(sequence number search)";
                                        break;
                                    case 7:
                                        strRet = "ReSTART(blinking)";
                                        break;
                                    case 8:
                                        strRet = "ReSET";
                                        break;
                                    case 9:
                                        break;
                                    case 10:
                                        break;
                                    case 11:
                                        break;
                                    case 12:
                                        break;
                                    case 13:
                                        strRet = "HPCC(during RISC operation)";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if ((num == "16") || (num == "18" && type == "W"))
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "NOT READY";
                                        break;
                                    case 1:
                                        strRet = "M-READY";
                                        break;
                                    case 2:
                                        strRet = "C-START";
                                        break;
                                    case 3:
                                        strRet = "F-HOLD";
                                        break;
                                    case 4:
                                        strRet = "B-STOP";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "****(reset)";
                                        break;
                                    case 1:
                                        strRet = "STOP";
                                        break;
                                    case 2:
                                        strRet = "HOLD";
                                        break;
                                    case 3:
                                        strRet = "STaRT";
                                        break;
                                    case 4:
                                        strRet =
                                            "MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI)";
                                        break;
                                    default:
                                        break;
                                }
                            }


                            ret.Value = strRet;
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

        //详细运行状态
        [Method("Fanuc", description: "读Fanuc设备详细运行状态")]
        public DriverReturnValueModel ReadDeviceDetailRunStatus(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBSYS a = new Focas1.ODBSYS();
                    _result = FanucFocas.cnc_sysinfo(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        string type = new string(a.mt_type);
                        string num = new string(a.cnc_type);
                        Focas1.ODBST aa = new Focas1.ODBST();
                        _result = FanucFocas.cnc_statinfo(_hndl, aa);
                        if (_result == FanucFocas.EW_OK)
                        {
                            var strRet = string.Empty;
                            var runRet = aa.run;
                            if (num == "15")
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "STOP";
                                        break;
                                    case 1:
                                        strRet = "HOLD";
                                        break;
                                    case 2:
                                        strRet = "STaRT";
                                        break;
                                    case 3:
                                        strRet = "MSTR(jog mdi)";
                                        break;
                                    case 4:
                                        strRet = "ReSTaRt(not blinking)";
                                        break;
                                    case 5:
                                        strRet = "PRSR(program restart)";
                                        break;
                                    case 6:
                                        strRet = "NSRC(sequence number search)";
                                        break;
                                    case 7:
                                        strRet = "ReSTaRt(blinking)";
                                        break;
                                    case 8:
                                        strRet = "ReSET";
                                        break;
                                    case 9:
                                        break;
                                    case 10:
                                        break;
                                    case 11:
                                        break;
                                    case 12:
                                        break;
                                    case 13:
                                        strRet = "HPCC(during RISC operation)";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if ((num == "16") || (num == "18" && type == "W"))
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "NOT READY";
                                        break;
                                    case 1:
                                        strRet = "M-READY";
                                        break;
                                    case 2:
                                        strRet = "C-START";
                                        break;
                                    case 3:
                                        strRet = "F-HOLD";
                                        break;
                                    case 4:
                                        strRet = "B-STOP";
                                        break;
                                    default:
                                        strRet = runRet.ToString();
                                        break;
                                }
                            }
                            else
                            {
                                switch (runRet)
                                {
                                    case 0:
                                        strRet = "****(reset)";
                                        break;
                                    case 1:
                                        strRet = "STOP";
                                        break;
                                    case 2:
                                        strRet = "HOLD";
                                        break;
                                    case 3:
                                        strRet = "STaRT";
                                        break;
                                    case 4:
                                        strRet =
                                            "MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI)";
                                        break;
                                    default:
                                        strRet = runRet.ToString();
                                        break;
                                }
                            }


                            ret.Value = strRet;
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

        //操作模式      cnc_statinfo    manual              
        //存在问题，未找到manual
        [Method("Fanuc", description: "读Fanuc设备操作模式")]
        public DriverReturnValueModel ReadDeviceModel(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBST aa = new Focas1.ODBST();
                    _result = FanucFocas.cnc_statinfo(_hndl, aa);
                    if (_result == FanucFocas.EW_OK)
                    {
                        var strRet = string.Empty;
                        var manualRet = aa.tmmode;

                        ret.Value = manualRet;
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

        //操作模式      cnc_statinfo    manual              
        //存在问题，未找到manual
        [Method("Fanuc", description: "读Fanuc宏变量")]
        public DriverReturnValueModel ReadHongNew(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {


                    short number = Convert.ToInt16(ioarg.Address);   //registerAddr == number
                    Focas1.ODBM odbm = new Focas1.ODBM();
                    short res = Focas1.cnc_rdmacro(_hndl, number, 10, odbm);
                    if (res == FanucFocas.EW_OK)
                    {

                        ret.Value = (Math.Pow(10, -odbm.dec_val) * odbm.mcr_val).ToString();
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

        //操作模式      cnc_statinfo    manual              
        //存在问题，未找到manual
        [Method("Fanuc", description: "读Fanuc寄存器")]
        public DriverReturnValueModel ReadStringNew(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    short AddrNum = Convert.ToInt16(ioarg.Address.Split(',')[1]);
                    byte[] valueReceive = new byte[AddrNum];
                    short number = Convert.ToInt16(ioarg.Address.Split(',')[0]);   //registerAddr == number
                    int iSizeConst = 2;//在委托代码和非委托代码之间传送的数据长度
                    int iSingle = AddrNum % iSizeConst;
                    int iLoop = iSingle == 0 ? AddrNum / iSizeConst : AddrNum / iSizeConst + 1;

                    ushort start = 0;
                    ushort end = 0;
                    // IODBPMC0结构体大小：头部8字节 + 数据部分5字节 = 13字节
                    // 但实际读取的数据长度是变化的，f参数应该是8 + 实际读取的字节数
                    ushort f = (ushort)(8 + iSizeConst);
                    for (int i = 0; i < iLoop; i++)
                    {
                        int newregisterAddr = number + i * iSizeConst;
                        int actualReadSize = iSizeConst;
                        if (iSingle == 0)
                        {
                            start = Convert.ToUInt16(newregisterAddr);
                            end = Convert.ToUInt16(newregisterAddr + iSizeConst - 1);
                            f = (ushort)(8 + iSizeConst);
                        }
                        else if (i == iLoop - 1)
                        {
                            start = Convert.ToUInt16(newregisterAddr);
                            end = Convert.ToUInt16(newregisterAddr + iSingle - 1);
                            actualReadSize = iSingle;
                            f = (ushort)(8 + iSingle);
                        }
                        else
                        {
                            start = Convert.ToUInt16(newregisterAddr);
                            end = Convert.ToUInt16(newregisterAddr + iSizeConst - 1);
                            f = (ushort)(8 + iSizeConst);
                        }

                        Focas1.IODBPMC0 iodbpmc0 = new Focas1.IODBPMC0();
                        // 确保cdata数组被初始化
                        if (iodbpmc0.cdata == null)
                        {
                            iodbpmc0.cdata = new byte[5];
                        }
                        
                        int res;
                        res = Focas1.pmc_rdpmcrng(_hndl, 9, 0, start, end, f, iodbpmc0);
                        if (res == Focas1.EW_OK)
                        {
                            // 检查cdata是否为空
                            if (iodbpmc0.cdata == null || iodbpmc0.cdata.Length == 0)
                            {
                                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                                ret.Message = $"读取失败: cdata为空";
                                _logger?.LogWarning($"ReadStringNew: cdata为空, start={start}, end={end}, f={f}");
                                break;
                            }
                            
                            var bytesToCopy = Math.Min(actualReadSize, iodbpmc0.cdata.Length);
                            for (int j = 0; j < bytesToCopy; j++)
                            {
                                var targetIndex = i * iSizeConst + j;
                                if (targetIndex >= valueReceive.Length) break;
                                valueReceive[targetIndex] = iodbpmc0.cdata[j];
                            }
                            
                            _logger?.LogDebug($"ReadStringNew: 读取成功, start={start}, end={end}, 读取字节数={bytesToCopy}, 数据={BitConverter.ToString(iodbpmc0.cdata, 0, bytesToCopy)}");
                        }
                        else
                        {
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败,错误码:{res}";
                            _logger?.LogWarning($"ReadStringNew: pmc_rdpmcrng失败, start={start}, end={end}, f={f}, 错误码={res}");
                            break;
                        }
                    }

                    // 无论成功还是失败，都将字节数组转换为字符串
                    var rawString = Encoding.ASCII.GetString(valueReceive).TrimEnd('\0', ' ');
                    
                    if (ret.StatusType == VaribaleStatusTypeEnum.Good)
                    {
                        ret.CookedValue = rawString;
                        ret.Value = rawString;
                        _logger?.LogDebug($"ReadStringNew: 最终结果, 地址={ioarg.Address}, 值={rawString}");
                    }
                    else
                    {
                        // 即使失败，也返回字符串格式，而不是字节数组
                        ret.Value = rawString;
                        ret.CookedValue = rawString;
                    }
                }

                catch (Exception ex)
                {
                    ret.StatusType = VaribaleStatusTypeEnum.Bad;
                    ret.Message = $"读取失败,{ex.Message}";
                    // 确保返回字符串而不是字节数组
                    try
                    {
                        byte[] valueReceive = new byte[0];
                        if (ioarg.Address.Contains(','))
                        {
                            short AddrNum = Convert.ToInt16(ioarg.Address.Split(',')[1]);
                            valueReceive = new byte[AddrNum];
                        }
                        ret.Value = Encoding.ASCII.GetString(valueReceive).TrimEnd('\0', ' ');
                        ret.CookedValue = ret.Value;
                    }
                    catch
                    {
                        ret.Value = string.Empty;
                        ret.CookedValue = string.Empty;
                    }
                    _logger?.LogError(ex, $"ReadStringNew异常, 地址={ioarg.Address}");
                }
            }
            else
            {
                ret.StatusType = VaribaleStatusTypeEnum.Bad;
                ret.Message = "连接失败";
                // 确保返回字符串而不是字节数组
                ret.Value = string.Empty;
                ret.CookedValue = string.Empty;
            }

            return ret;
        }

        //主轴转速      cnc_acts
        [Method("Fanuc", description: "读Fanuc设备实际主轴转速")]
        public DriverReturnValueModel ReadDeviceActSpindleSpeed(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBACT a = new Focas1.ODBACT();
                    _result = FanucFocas.cnc_acts(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.data;
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

        //供给速度      cnc_actf
        [Method("Fanuc", description: "读Fanuc设备实际进给速度")]
        public DriverReturnValueModel ReadDeviceActFeedRate(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };

            if (IsConnected)
            {
                try
                {
                    Focas1.ODBACT a = new Focas1.ODBACT();
                    _result = FanucFocas.cnc_actf(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.data;
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

        //主轴倍率      cnc_rdopnlsgnl
        [Method("Fanuc", description: "读Fanuc设备主轴倍率")]
        public DriverReturnValueModel ReadDeviceSpindleOvr(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBSGNL a = new Focas1.IODBSGNL();
                    _result = FanucFocas.cnc_rdopnlsgnl(_hndl, 0, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.spdl_ovrd;
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

        //进给倍率      cnc_rdopnlsgnl
        [Method("Fanuc", description: "读Fanuc设备进给倍率")]
        public DriverReturnValueModel ReadDeviceFeedOvr(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBSGNL a = new Focas1.IODBSGNL();
                    _result = FanucFocas.cnc_rdopnlsgnl(_hndl, 0, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.feed_ovrd;
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

        //设备异常代码：报警号        cnc_rdalmmsg2
        [Method("Fanuc", description: "读Fanuc设备报警号")]
        public DriverReturnValueModel ReadDeviceAlarmNum(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    short inInt = 1;
                    Focas1.ODBALMMSG2 a = new Focas1.ODBALMMSG2();
                    _result = FanucFocas.cnc_rdalmmsg2(_hndl, -1, ref inInt, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.msg1.alm_no;
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

        //设备异常信息：报警文本
        [Method("Fanuc", description: "读Fanuc设备报警文本")]
        public DriverReturnValueModel ReadDeviceAlarmText(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    short inInt = 1;
                    Focas1.ODBALMMSG2 a = new Focas1.ODBALMMSG2();
                    _result = FanucFocas.cnc_rdalmmsg2(_hndl, -1, ref inInt, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = new string(a.msg1.alm_msg);
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

        //异常类型
        [Method("Fanuc", description: "读Fanuc设备报警类型")]
        public DriverReturnValueModel ReadDeviceAlarmType(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    short inInt = 1;
                    Focas1.ODBALMMSG2 a = new Focas1.ODBALMMSG2();
                    _result = FanucFocas.cnc_rdalmmsg2(_hndl, -1, ref inInt, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        short alarmType = -1;
                        var alarmStrType = string.Empty;
                        switch (a.msg1.type)
                        {
                            case 0:
                                alarmStrType = "Parameter switch on";
                                break;
                            case 1:
                                alarmStrType = "Power off parameter set";
                                break;
                            case 2:
                                alarmStrType = "I/O error";
                                break;
                            case 3:
                                alarmStrType = "Foreground P/S";
                                break;
                            case 4:
                                alarmStrType = "Overtravel,External data";
                                break;
                            case 5:
                                alarmStrType = "Overheat alarm";
                                break;
                            case 6:
                                alarmStrType = "Servo alarm";
                                break;
                            case 7:
                                alarmStrType = "Data I/O error";
                                break;
                            case 8:
                                alarmStrType = "Macro alarm";
                                break;
                            case 9:
                                alarmStrType = "Macro alarm";
                                break;
                            case 10:
                                alarmStrType = "Other alarm(DS)";
                                break;
                            case 11:
                                alarmStrType = "Alarm concerning Malfunction prevent functions (IE) ";
                                break;
                            case 12:
                                alarmStrType = "Background P/S (BG) ";
                                break;
                            case 13:
                                alarmStrType = "Syncronized error (SN) ";
                                break;
                            case 14:
                                alarmStrType = "(reserved)";
                                break;
                            case 15:
                                alarmStrType = "External alarm message (EX) ";
                                break;
                            case 16:
                                alarmStrType = "(reserved)";
                                break;
                            case 17:
                                alarmStrType = "(reserved)";
                                break;
                            case 18:
                                alarmStrType = "(reserved)";
                                break;
                            case 19:
                                alarmStrType = "PMC error (PC) ";
                                break;
                            case -1:
                                alarmStrType = "All type";
                                break;

                            default:
                                alarmStrType = a.msg1.type.ToString();
                                break;
                        }

                        ret.Value = alarmStrType;
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

        //执行程序名     cnc_rdprgnum
        [Method("Fanuc", description: "读Fanuc设备执行程序号")]
        public DriverReturnValueModel ReadDeviceExeProgamNumber(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.ODBPRO a = new Focas1.ODBPRO();
                    _result = FanucFocas.cnc_rdprgnum(_hndl, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.data;
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

        //工件计数
        [Method("Fanuc", description: "读Fanuc设备计数器值")]
        public DriverReturnValueModel ReadDeviceCountVaule(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    int a = 0;
                    _result = FanucFocas.cnc_rdblkcount(_hndl, out a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a;
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

        //刀具号
        [Method("Fanuc", description: "读Fanuc设备刀具号")]
        public DriverReturnValueModel ReadDeviceToolNumber(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.ODBTLIFE4 a = new Focas1.ODBTLIFE4();
                    _result = FanucFocas.cnc_toolnum(_hndl, 0, 0, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.data;
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

        //上电总时间     cnc_rdtimer
        [Method("Fanuc", description: "读Fanuc设备上电时间")]
        public DriverReturnValueModel ReadDevicePowerTime(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBTIME a = new Focas1.IODBTIME();
                    _result = FanucFocas.cnc_rdtimer(_hndl, 0, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.minute;
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

        //运行总时间
        [Method("Fanuc", description: "读Fanuc设备运行时间")]
        public DriverReturnValueModel ReadDeviceOperateTime(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBTIME a = new Focas1.IODBTIME();
                    _result = FanucFocas.cnc_rdtimer(_hndl, 1, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.minute * 60 + a.msec;
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

        //切削总时间
        [Method("Fanuc", description: "读Fanuc设备切削时间")]
        public DriverReturnValueModel ReadDeviceCutTime(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBTIME a = new Focas1.IODBTIME();
                    _result = FanucFocas.cnc_rdtimer(_hndl, 2, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.minute * 60 + a.msec;
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

        //循环时间
        [Method("Fanuc", description: "读Fanuc设备循环时间")]
        public DriverReturnValueModel ReadDeviceCycleTime(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    Focas1.IODBTIME a = new Focas1.IODBTIME();
                    _result = FanucFocas.cnc_rdtimer(_hndl, 2, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.minute * 60 + a.msec;
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

        //主轴负载      cnc_rdspmeter
        [Method("Fanuc", description: "读Fanuc设备主轴负载")]
        public DriverReturnValueModel ReadDeviceSpindle(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    short num = 4;
                    Focas1.ODBSPLOAD a = new Focas1.ODBSPLOAD();
                    _result = FanucFocas.cnc_rdspmeter(_hndl, 0, ref num, a);
                    if (_result == FanucFocas.EW_OK)
                    {
                        ret.Value = a.spload1.spload.data * Math.Pow(10, a.spload1.spload.dec);
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


        /// <summary>
        /// 无用方法
        /// </summary>
        /// <param name="ioarg"></param>
        /// <returns></returns>
        public DriverReturnValueModel Read(DriverAddressIoArgModel ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            return ret;
        }

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="method"></param>
        /// <param name="ioarg"></param>
        /// <returns></returns>
        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            await Task.CompletedTask;
            return rpcResponse;
        }
        #endregion

    }
}
