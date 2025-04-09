using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using Plugin.PlatformHandler;
using PluginInterface;
using PluginInterface.IoTSharp;
using System.Collections.Concurrent;

namespace Plugin
{
    public class MessageService
    {
        private readonly ILogger<MessageService> _logger;
        private IPlatformHandler _platformHandler;
        private SystemConfig _systemConfig;
        private ManagedMqttClientOptions _options;
        private IManagedMqttClient? Client { get; set; }

        public bool IsConnected => Client?.IsConnected ?? false;

        public event EventHandler<RpcRequest>? OnExcRpc;

        public event EventHandler<ISAttributeResponse>? OnReceiveAttributes;

        private readonly string _tbRpcTopic = "v1/gateway/rpc";

        // 使用线程安全的集合确保并发访问安全
        private readonly ConcurrentDictionary<string, List<PayLoad>> _lastTelemetrys = new();

        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
            // 异步启动客户端，避免构造函数阻塞
            _ = StartClientAsync();
        }

        public async Task StartClientAsync()
        {
            try
            {
                if (Client != null)
                {
                    await Client.StopAsync().ConfigureAwait(false);
                    Client.Dispose();
                }

                Client = new MqttFactory().CreateManagedMqttClient();

                // 使用 using 声明，确保资源及时释放
                await using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
                _systemConfig = await dc.Set<SystemConfig>().FirstOrDefaultAsync().ConfigureAwait(false)
                                ?? throw new Exception("系统配置未找到");

                #region ClientOptions

                _options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithMaxPendingMessages(100000)
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(string.IsNullOrEmpty(_systemConfig.ClientId) ? Guid.NewGuid().ToString() : _systemConfig.ClientId)
                        .WithTcpServer(_systemConfig.MqttIp, _systemConfig.MqttPort)
                        .WithCredentials(_systemConfig.MqttUName, _systemConfig.MqttUPwd)
                        .WithTimeout(TimeSpan.FromSeconds(30))
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                        .WithProtocolVersion(MqttProtocolVersion.V311)
                        .WithCleanSession(true)
                        .Build())
                    .Build();

                #endregion ClientOptions

                Client.ConnectedAsync += Client_ConnectedAsync;
                Client.DisconnectedAsync += Client_DisconnectedAsync;
                Client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;

                await Client.StartAsync(_options).ConfigureAwait(false);

                // 使用工厂模式创建对应平台的处理器
                _platformHandler = PlatformHandlerFactory.CreateHandler(_systemConfig.IoTPlatformType, Client, _logger, OnExcRpc);
                _platformHandler.OnExcRpc += (sender, request) => OnExcRpc?.Invoke(sender, request);

                _logger.LogInformation("MQTT WAITING FOR APPLICATION MESSAGES");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartClientAsync FAILED");
            }
        }

        public async Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _logger.LogInformation("MQTT CONNECTED WITH SERVER");
            try
            {
                await _platformHandler.ClientConnected().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT Subscribe FAILED");
            }
        }

        public Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogError("MQTT DISCONNECTED WITH SERVER");
            return Task.CompletedTask;
        }

        public async Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogDebug($"ApplicationMessageReceived Topic: {e.ApplicationMessage.Topic}");
            try
            {
                _platformHandler.ReceiveRpc(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ClientId:{e.ClientId} Topic:{e.ApplicationMessage.Topic}");
            }
        }

        /// <summary>
        /// 判断是否需要发布遥测数据（仅在数据有更新或超过规定时间间隔时发布）。
        /// </summary>
        private bool CanPubTelemetry(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            try
            {
                // 缓存本次待发送的数据，减少重复查找
                if (!sendModel.TryGetValue(deviceName, out var newTelemetry) || newTelemetry.Count == 0)
                {
                    return false;
                }

                // 尝试获取上一次的遥测数据
                if (!_lastTelemetrys.TryGetValue(deviceName, out var lastTelemetry))
                {
                    _lastTelemetrys[deviceName] = newTelemetry;
                    return true;
                }

                var newPayload = newTelemetry[0];
                var lastPayload = lastTelemetry[0];

                if (device.CgUpload)
                {
                    if (newPayload.TS - lastPayload.TS > device.EnforcePeriod ||
                        !newPayload.Values.SequenceEqual(lastPayload.Values))
                    {
                        _lastTelemetrys[deviceName] = newTelemetry;
                        return true;
                    }
                }
                else
                {
                    _lastTelemetrys[deviceName] = newTelemetry;
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in CanPubTelemetry");
                return true;
            }

            return false;
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            if (CanPubTelemetry(deviceName, device, sendModel))
            {
                await _platformHandler.PublishTelemetryAsync(deviceName, device, sendModel).ConfigureAwait(false);
            }
        }

        public async Task UploadAttributeAsync(string deviceName, object obj) =>
            await _platformHandler.UploadAttributeAsync(deviceName, obj).ConfigureAwait(false);

        public async Task DeviceConnected(string deviceName, Device device) =>
            await _platformHandler.DeviceConnected(deviceName, device).ConfigureAwait(false);

        public async Task DeviceDisconnected(string deviceName, Device device) =>
            await _platformHandler.DeviceDisconnected(deviceName, device).ConfigureAwait(false);

        public async Task ResponseRpcAsync(RpcResponse rpcResponse) =>
            await _platformHandler.ResponseRpcAsync(rpcResponse).ConfigureAwait(false);

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args) =>
            await _platformHandler.RequestAttributes(deviceName, anySide, args).ConfigureAwait(false);

        public async Task DeviceAdded(Device device) =>
            await _platformHandler.DeviceAdded(device).ConfigureAwait(false);

        public async Task DeviceDeleted(Device device) =>
            await _platformHandler.DeviceDeleted(device).ConfigureAwait(false);
    }
}