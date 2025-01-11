using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
using PluginInterface.IoTSharp;
using PluginInterface;
using IoTGateway.DataAccess;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet;
using MQTTnet.Protocol;
using Plugin.PlatformHandler;
using Newtonsoft.Json;
using Quartz.Logging;

namespace Plugin
{
    public class MessageService
    {
        private readonly ILogger<MessageService> _logger;
        private IPlatformHandler _platformHandler;

        private SystemConfig _systemConfig;
        private ManagedMqttClientOptions _options;
        public bool IsConnected => (Client.IsConnected);
        private IManagedMqttClient? Client { get; set; }
        public event EventHandler<RpcRequest> OnExcRpc;
        public event EventHandler<ISAttributeResponse> OnReceiveAttributes;
        private readonly string _tbRpcTopic = "v1/gateway/rpc";
        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;

            StartClientAsync().Wait();

        }

        public async Task StartClientAsync()
        {
            try
            {
                if (Client != null)
                {
                    Client.Dispose();
                }
                Client = new MqttFactory().CreateManagedMqttClient();
                await using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
                _systemConfig = dc.Set<SystemConfig>().First();

                #region ClientOptions

                _options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithMaxPendingMessages(100000)
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(string.IsNullOrEmpty(_systemConfig.ClientId)
                            ? Guid.NewGuid().ToString()
                            : _systemConfig.ClientId)
                        .WithTcpServer(_systemConfig.MqttIp, _systemConfig.MqttPort)
                        .WithCredentials(_systemConfig.MqttUName, _systemConfig.MqttUPwd)
                        .WithTimeout(TimeSpan.FromSeconds(30))
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                        .WithProtocolVersion(MqttProtocolVersion.V311)
                        .WithCleanSession(true)
                        .Build())
                    .Build();
                #endregion

                Client.ConnectedAsync += Client_ConnectedAsync;
                Client.DisconnectedAsync += Client_DisconnectedAsync;
                Client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;

                await Client.StartAsync(_options);

                // 使用工厂模式创建对应平台的处理器
                _platformHandler = PlatformHandlerFactory.CreateHandler(_systemConfig.IoTPlatformType, Client, _logger, OnExcRpc);

                _logger.LogInformation("MQTT WAITING FOR APPLICATION MESSAGES");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StartManagedClientAsync FAILED ");
            }
        }

        public async Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _logger.LogInformation($"MQTT CONNECTED WITH SERVER ");
            #region Topics
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        //{"device": "Device A", "data": {"id": $request_id, "method": "toggle_gpio", "params": {"pin":1}}}
                        await Client.SubscribeAsync(_tbRpcTopic, MqttQualityOfServiceLevel.ExactlyOnce);
                        //Message: {"id": $request_id, "device": "Device A", "value": "value1"}
                        await Client.SubscribeAsync("v1/gateway/attributes/response", MqttQualityOfServiceLevel.ExactlyOnce);
                        //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
                        await Client.SubscribeAsync("v1/gateway/attributes", MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                    case IoTPlatformType.IoTSharp:
                    case IoTPlatformType.IoTGateway:
                        await Client.SubscribeAsync("devices/+/rpc/request/+/+", MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.SubscribeAsync("devices/+/attributes/update", MqttQualityOfServiceLevel.ExactlyOnce);
                        //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
                        await Client.SubscribeAsync("devices/+/attributes/response/+", MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                    case IoTPlatformType.ThingsCloud:
                        await Client.SubscribeAsync("gateway/attributes/response", MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.SubscribeAsync("gateway/attributes/get/response", MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.SubscribeAsync("gateway/attributes/push", MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.SubscribeAsync("gateway/event/response", MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.SubscribeAsync("gateway/command/send", MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT Subscribe FAILED");
            }
            #endregion
        }

        public async Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            try
            {
                _logger.LogError($"MQTT DISCONNECTED WITH SERVER ");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT CONNECTING FAILED");
            }
        }

        public Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogDebug(
                $"ApplicationMessageReceived Topic {e.ApplicationMessage.Topic}  QualityOfServiceLevel:{e.ApplicationMessage.QualityOfServiceLevel} Retain:{e.ApplicationMessage.Retain} ");
            try
            {
                _platformHandler.ReceiveRpc(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, $"ClientId:{e.ClientId} Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}");
            }

            return Task.CompletedTask;
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device,
            Dictionary<string, List<PayLoad>> sendModel)
        {
            if (CanPubTelemetry(deviceName, device, sendModel))
            {
                await _platformHandler.PublishTelemetryAsync(deviceName, device, sendModel);
            }
        }

        public async Task UploadAttributeAsync(string deviceName, object obj)
        {
            await _platformHandler.UploadAttributeAsync(deviceName, obj);
        }

        public async Task DeviceConnected(string deviceName, Device device)
        {
            try
            {
                await _platformHandler.DeviceConnected(deviceName, device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeviceConnected:{deviceName}");
            }
        }

        public async Task DeviceDisconnected(string deviceName, Device device)
        {
            try
            {
                await _platformHandler.DeviceDisconnected(deviceName, device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeviceDisconnected:{deviceName}");
            }
        }

        public async Task ResponseRpcAsync(RpcResponse rpcResponse)
        {
            try
            {
                await _platformHandler.ResponseRpcAsync(rpcResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ResponseRpc Error,{rpcResponse}");
            }
        }

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {

            await _platformHandler.RequestAttributes(deviceName, anySide, args);
        }


        public async Task DeviceAdded(Device device)
        {
            try
            {
                await _platformHandler.DeviceAdded(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeviceAdded:{device.DeviceName}");
            }
        }

        public async Task DeviceDeleted(Device device)
        {
            try
            {
                await _platformHandler.DeviceDeleted(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeviceAdded:{device.DeviceName}");
            }
        }


        private Dictionary<string, List<PayLoad>> _lastTelemetrys = new(0);

        /// <summary>
        /// 判断是否推送遥测数据
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="device">设备</param>
        /// <param name="sendModel">遥测</param>
        /// <returns></returns>
        private bool CanPubTelemetry(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            bool canPub = false;
            try
            {//第一次上传
                if (!_lastTelemetrys.ContainsKey(deviceName))
                    canPub = true;
                else
                {
                    //变化上传
                    if (device.CgUpload)
                    {
                        //是否超过归档周期
                        if (sendModel[deviceName][0].TS - _lastTelemetrys[deviceName][0].TS >
                            device.EnforcePeriod)
                            canPub = true;
                        //是否变化 这里不好先用
                        else
                        {
                            if (JsonConvert.SerializeObject(sendModel[deviceName][0].Values) !=
                                JsonConvert.SerializeObject(_lastTelemetrys[deviceName][0].Values))
                                canPub = true;
                        }
                    }
                    //非变化上传
                    else
                        canPub = true;
                }
            }
            catch (Exception e)
            {
                canPub = true;
                Console.WriteLine(e);
            }

            if (canPub)
                _lastTelemetrys[deviceName] = sendModel[deviceName];
            return canPub;
        }
    }
}
