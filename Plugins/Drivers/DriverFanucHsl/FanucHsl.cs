
using PluginInterface;
using System;
using System.Text;
using HslCommunication.CNC.Fanuc;
using HslCommunication;

namespace DriverFanucHsl
{
    [DriverSupported("Fanuc-0i")]
    [DriverInfoAttribute("Fanuc-0i", "V11.0.0", "Copyright HSL ")]
    public class FanucHsl : IDriver
    {
        private FanucSeries0i fanuc;
        #region 配置参数

        [ConfigParameter("设备Id")]
        public Guid DeviceId { get; set; }

        [ConfigParameter("IP地址")]
        public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")]
        public int Port { get; set; } = 8193;

        [ConfigParameter("超时时间ms")]
        public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")]
        public uint MinPeriod { get; set; } = 3000;

        #endregion

        public FanucHsl(Guid deviceId)
        {
            // 授权示例 Authorization example
            if (!Authorization.SetAuthorizationCode("输入你的授权号"))
            {
                //return;   // 激活失败应该退出系统
            }

            DeviceId = deviceId;
        }


        public bool IsConnected
        {
            get
            {
                if (fanuc == null)
                    return false;
                else
                {
                    OperateResult<int[]> read = fanuc.ReadProgramList();
                    if (read.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public bool Connect()
        {
            try
            {
                fanuc?.ConnectClose();
                fanuc = new FanucSeries0i(IpAddress, Port);
                return fanuc.ConnectServer().IsSuccess;

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
                fanuc.ConnectClose();
                return !IsConnected;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void Dispose()
        {

        }

        [Method("Fanuc", description: "读系统状态")]
        public DriverReturnValueModel ReadSysStatusInfo(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysStatusInfo> read = fanuc.ReadSysStatusInfo();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(read.Content);
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "读报警信息")]
        public DriverReturnValueModel ReadSystemAlarm(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysAlarm[]> read = fanuc.ReadSystemAlarm();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(read.Content);
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "坐标数据")]
        public DriverReturnValueModel ReadSysAllCoors(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysAllCoors> read = fanuc.ReadSysAllCoors();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(read.Content);
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "程序列表")]
        public DriverReturnValueModel ReadProgramList(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int[]> read = fanuc.ReadProgramList();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "当前程序名")]
        public DriverReturnValueModel ReadSystemProgramCurrent(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string, int> read = fanuc.ReadSystemProgramCurrent();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new Dictionary<string, object>() { { "ProgramName", read.Content1 }, { "ProgramNo", read.Content2 } }
                            );
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "主轴转进速")]
        public DriverReturnValueModel ReadSpindleSpeedAndFeedRate(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double, double> read = fanuc.ReadSpindleSpeedAndFeedRate();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new Dictionary<string, object>() { { "SpindleSpeed", read.Content1 }, { "FeedRate", read.Content2 } }
                        );
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "伺服负载")]
        public DriverReturnValueModel ReadFanucAxisLoad(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double[]> read = fanuc.ReadFanucAxisLoad();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "道具补偿")]
        public DriverReturnValueModel ReadCutterInfos(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<CutterInfo[]> read = fanuc.ReadCutterInfos();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(read.Content);
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "程序路径")]
        public DriverReturnValueModel ReadCurrentForegroundDir(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string> read = fanuc.ReadCurrentForegroundDir();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "工件尺寸")]
        public DriverReturnValueModel ReadDeviceWorkPiecesSize(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double[]> read = fanuc.ReadDeviceWorkPiecesSize();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "报警代号")]
        public DriverReturnValueModel ReadAlarmStatus(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = fanuc.ReadAlarmStatus();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "机床时间")]
        public DriverReturnValueModel ReadCurrentDateTime(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<DateTime> read = fanuc.ReadCurrentDateTime();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "已加工数量")]
        public DriverReturnValueModel ReadCurrentProduceCount(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = fanuc.ReadCurrentProduceCount();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "总加工数量")]
        public DriverReturnValueModel ReadExpectProduceCount(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = fanuc.ReadExpectProduceCount();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "系统语言")]
        public DriverReturnValueModel ReadLanguage(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<ushort> read = fanuc.ReadLanguage();
                    if (read.IsSuccess)
                        ret.Value = (LanguageType)read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "当前程序")]
        public DriverReturnValueModel ReadCurrentProgram(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string> read = fanuc.ReadCurrentProgram();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "开机时间")]
        public DriverReturnValueModel ReadOnLineTime(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = fanuc.ReadTimeData(0);
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "运行时间")]
        public DriverReturnValueModel ReadRunTime(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = fanuc.ReadTimeData(1);
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "切割时间")]
        public DriverReturnValueModel ReadCuttingTime(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = fanuc.ReadTimeData(2);
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "空闲时间")]
        public DriverReturnValueModel ReadIdleTime(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = fanuc.ReadTimeData(3);
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "当前道具号")]
        public DriverReturnValueModel ReadCutterNumber(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = fanuc.ReadCutterNumber();
                    if (read.IsSuccess)
                        ret.Value = read.Content;
                    else
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "读宏变量")]
        public DriverReturnValueModel ReadSystemMacroValue(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    if (!int.TryParse(Ioarg.Address, out int address))
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"宏变量地址错误";
                    }
                    else
                    {
                        OperateResult<double> read = fanuc.ReadSystemMacroValue(address);
                        if (read.IsSuccess)
                            ret.Value = read.Content;
                        else
                        {
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败";
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

        [Method("Fanuc", description: "读取程序")]
        public DriverReturnValueModel ReadProgramAsync(DriverAddressIoArgModel Ioarg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    if (!int.TryParse(Ioarg.Address, out int address))
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"程序号错误";
                    }
                    else
                    {
                        OperateResult<string> read = fanuc.ReadProgram(address);
                        if (read.IsSuccess)
                            ret.Value = read.Content;
                        else
                        {
                            ret.StatusType = VaribaleStatusTypeEnum.Bad;
                            ret.Message = $"读取失败";
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

        public DriverReturnValueModel Read(DriverAddressIoArgModel Ioarg)
        {
            throw new NotImplementedException();
        }

        private enum LanguageType
        {
            英语 = 0,
            日语 = 1,
            德语 = 2,
            法语 = 3,
            中文简繁体 = 4,
            韩语 = 6,
            中文简体 = 15,
            俄语 = 16,
            土耳其语 = 17
        }
    }
}