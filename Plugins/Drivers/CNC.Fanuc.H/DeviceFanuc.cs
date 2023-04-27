using PluginInterface;
using HslCommunication;
using HslCommunication.CNC.Fanuc;
using Microsoft.Extensions.Logging;

namespace CNC.Fanuc.H
{
    [DriverSupported("Series0i")]
    [DriverInfo("Fanuc", "V1.0.0", "Copyright iotgateway.net 20230220")]
    public class DeviceFanuc : IDriver
    {
        private FanucSeries0i _fanuc;

        public ILogger _logger { get; set; }
        private readonly string _device;

        #region 配置参数

        [ConfigParameter("设备Id")] public string DeviceId { get; set; }

        [ConfigParameter("IP地址")] public string IpAddress { get; set; } = "127.0.0.1";

        [ConfigParameter("端口号")] public int Port { get; set; } = 8193;

        [ConfigParameter("超时时间ms")] public int Timeout { get; set; } = 3000;

        [ConfigParameter("最小通讯周期ms")] public uint MinPeriod { get; set; } = 3000;

        #endregion

        #region 生命周期

        public DeviceFanuc(string device, ILogger logger)
        {
            //输入授权码，禁止非法使用
            if (!Authorization.SetAuthorizationCode(""))
                return;

            _device = device;
            _logger = logger;


            _logger.LogInformation($"Device:[{_device}],Create()");
        }

        public bool IsConnected
        {
            get
            {
                if (_fanuc == null)
                    return false;

                OperateResult<int[]> read = _fanuc.ReadProgramList();
                if (read.IsSuccess)
                {
                    return true;
                }

                return false;
            }
        }

        public bool Connect()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Connect()");

                _fanuc = new FanucSeries0i(IpAddress, Port);
                return _fanuc.ConnectServer().IsSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Connect()");
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                _logger.LogInformation($"Device:[{_device}],Close()");
                _fanuc.ConnectClose();
                return !IsConnected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Close()");
                return false;
            }
        }

        public void Dispose()
        {
            try
            {
                _logger?.LogInformation($"Device:[{_device}],Dispose()");
                _fanuc?.Dispose();

                // Suppress finalization.
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Device:[{_device}],Dispose()");
            }

        }
        #endregion

        #region 读写方法


        [Method("Fanuc", description: "读系统状态")]
        public DriverReturnValueModel ReadSysStatusInfo(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysStatusInfo> read = _fanuc.ReadSysStatusInfo();
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
        public DriverReturnValueModel ReadSystemAlarm(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysAlarm[]> read = _fanuc.ReadSystemAlarm();
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
        public DriverReturnValueModel ReadSysAllCoors(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<SysAllCoors> read = _fanuc.ReadSysAllCoors();
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
        public DriverReturnValueModel ReadProgramList(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int[]> read = _fanuc.ReadProgramList();
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
        public DriverReturnValueModel ReadSystemProgramCurrent(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string, int> read = _fanuc.ReadSystemProgramCurrent();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new Dictionary<string, object>()
                                { { "ProgramName", read.Content1 }, { "ProgramNo", read.Content2 } }
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
        public DriverReturnValueModel ReadSpindleSpeedAndFeedRate(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double, double> read = _fanuc.ReadSpindleSpeedAndFeedRate();
                    if (read.IsSuccess)
                        ret.Value = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new Dictionary<string, object>()
                                { { "SpindleSpeed", read.Content1 }, { "FeedRate", read.Content2 } }
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
        public DriverReturnValueModel ReadFanucAxisLoad(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double[]> read = _fanuc.ReadFanucAxisLoad();
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
        public DriverReturnValueModel ReadCutterInfos(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<CutterInfo[]> read = _fanuc.ReadCutterInfos();
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
        public DriverReturnValueModel ReadCurrentForegroundDir(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string> read = _fanuc.ReadCurrentForegroundDir();
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
        public DriverReturnValueModel ReadDeviceWorkPiecesSize(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<double[]> read = _fanuc.ReadDeviceWorkPiecesSize();
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
        public DriverReturnValueModel ReadAlarmStatus(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = _fanuc.ReadAlarmStatus();
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
        public DriverReturnValueModel ReadCurrentDateTime(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<DateTime> read = _fanuc.ReadCurrentDateTime();
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
        public DriverReturnValueModel ReadCurrentProduceCount(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = _fanuc.ReadCurrentProduceCount();
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
        public DriverReturnValueModel ReadExpectProduceCount(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = _fanuc.ReadExpectProduceCount();
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
        public DriverReturnValueModel ReadLanguage(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<ushort> read = _fanuc.ReadLanguage();
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
        public DriverReturnValueModel ReadCurrentProgram(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<string> read = _fanuc.ReadCurrentProgram();
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
        public DriverReturnValueModel ReadOnLineTime(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = _fanuc.ReadTimeData(0);
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
        public DriverReturnValueModel ReadRunTime(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = _fanuc.ReadTimeData(1);
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
        public DriverReturnValueModel ReadCuttingTime(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = _fanuc.ReadTimeData(2);
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
        public DriverReturnValueModel ReadIdleTime(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<long> read = _fanuc.ReadTimeData(3);
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
        public DriverReturnValueModel ReadCutterNumber(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    OperateResult<int> read = _fanuc.ReadCutterNumber();
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
        public DriverReturnValueModel ReadSystemMacroValue(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    if (!int.TryParse(ioArg.Address, out int address))
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"宏变量地址错误";
                    }
                    else
                    {
                        OperateResult<double> read = _fanuc.ReadSystemMacroValue(address);
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
        public DriverReturnValueModel ReadProgramAsync(DriverAddressIoArgModel ioArg)
        {
            var ret = new DriverReturnValueModel { StatusType = VaribaleStatusTypeEnum.Good };
            if (IsConnected)
            {
                try
                {
                    if (!int.TryParse(ioArg.Address, out int address))
                    {
                        ret.StatusType = VaribaleStatusTypeEnum.Bad;
                        ret.Message = $"程序号错误";
                    }
                    else
                    {
                        OperateResult<string> read = _fanuc.ReadProgram(address);
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


        public async Task<RpcResponse> WriteAsync(string requestId, string method, DriverAddressIoArgModel ioArg)
        {
            RpcResponse rpcResponse = new() { IsSuccess = false, Description = "设备驱动内未实现写入功能" };
            return rpcResponse;
        }

        #endregion


        public DriverReturnValueModel Read(DriverAddressIoArgModel ioArg)
        {
            return new DriverReturnValueModel(VaribaleStatusTypeEnum.MethodError);
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