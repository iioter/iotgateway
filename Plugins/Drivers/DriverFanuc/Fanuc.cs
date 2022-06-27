using PluginInterface;

namespace DriverFaunc
{
    [DriverSupported("Fanuc-0i")]
    [DriverInfoAttribute("Fanuc-0i", "V1.0.0", "Copyright IoTGateway© 2022-05-25")]
    public class Fanuc : IDriver
    {
        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; } = Guid.NewGuid();
        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;
        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;
        [ConfigParameter("设备Ip")]
        public string DeviceIp { get; set; } = "127.0.0.1";
        [ConfigParameter("设备Port")]
        public int DevicePort { get; set; } = 8193;

        private ushort _hndl;
        private int _result = -1;

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

        public Fanuc(Guid deviceId)
        {
            DeviceId = deviceId;
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

        public bool Connect()
        {
            try
            {
                _result = -1;
                _result = FanucFocas.cnc_allclibhndl3(DeviceIp, Convert.ToUInt16(DevicePort), Convert.ToInt32(Timeout), out _hndl);

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

        }

        #region 无用方法
        public DriverReturnValueModel Read(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            return ret;
        }

        public Task<RpcResponse> WriteAsync(string RequestId, string Method, DriverAddressIoArgModel Ioarg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return Task.FromResult(rpcResponse);
        }
        #endregion

        //设备类型
        [Method("Fanuc", description: "读Fanuc设备类型")]
        public DriverReturnValueModel ReadDeviceType(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceRunStatus(DriverAddressIoArgModel Ioarg)
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
                                        strRet = "MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI)";
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
        public DriverReturnValueModel ReadDeviceDetailRunStatus(DriverAddressIoArgModel Ioarg)
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
                                        strRet = "MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI)";
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
        //操作模式      cnc_statinfo    manual              
        //存在问题，未找到manual
        [Method("Fanuc", description: "读Fanuc设备操作模式")]
        public DriverReturnValueModel ReadDeviceModel(DriverAddressIoArgModel Ioarg)
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
        //主轴转速      cnc_acts
        [Method("Fanuc", description: "读Fanuc设备实际主轴转速")]
        public DriverReturnValueModel ReadDeviceActSpindleSpeed(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceActFeedRate(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceSpindleOvr(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceFeedOvr(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceAlarmNum(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceAlarmText(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceAlarmType(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceExeProgamNumber(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceCountVaule(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceToolNumber(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDevicePowerTime(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceOperateTime(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceCutTime(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceCycleTime(DriverAddressIoArgModel Ioarg)
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
        public DriverReturnValueModel ReadDeviceSpindle(DriverAddressIoArgModel Ioarg)
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

    }
}
