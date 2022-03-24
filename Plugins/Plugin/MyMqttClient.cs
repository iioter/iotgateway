using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using PluginInterface;
using Quickstarts.ReferenceServer;

namespace Plugin
{
    public class MyMqttClient
    {
        private readonly ILogger<MyMqttClient> _logger;
        private readonly ReferenceNodeManager _uaNodeManager = null;

        private SystemConfig? _systemConfig;
        private IMqttClientOptions clientOptions;
        public bool IsConnected => (Client?.IsConnected).GetValueOrDefault();
        private IMqttClient Client { get; set; }
        public event EventHandler<RpcRequest> OnExcRpc;
        public event EventHandler<AttributeResponse> OnReceiveAttributes;
        public MyMqttClient(UAService uaService, ILogger<MyMqttClient> logger)
        {
            _logger = logger;
            _uaNodeManager = uaService.server.m_server.nodeManagers[0] as ReferenceNodeManager;
            ConnectAsync();
        }

        public async Task<bool> ConnectAsync()
        {
            bool initok = false;
            try
            {
                using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
                {
                    _systemConfig = DC.Set<SystemConfig>().FirstOrDefault();
                    if (_systemConfig == null)
                    {
                        _systemConfig = new SystemConfig()
                        {
                            ID = Guid.NewGuid(),
                            GatewayName = "iotgateway",
                            MqttIp = "localhost",
                            MqttPort = 1888,
                            MqttUName = "user",
                            MqttUPwd = "pwd",
                            IoTPlatformType = IoTPlatformType.IoTSharp
                        };
                        DC.Set<SystemConfig>().Add(_systemConfig);
                        DC.SaveChanges();
                    }
                    var factory = new MqttFactory();
                    Client = (MqttClient)factory.CreateMqttClient();
                    clientOptions = new MqttClientOptionsBuilder()
                           .WithClientId(_systemConfig.GatewayName + Guid.NewGuid().ToString())
                           .WithTcpServer(_systemConfig.MqttIp, _systemConfig.MqttPort)
                           .WithCredentials(_systemConfig.MqttUName, _systemConfig.MqttUPwd)
                           .WithCommunicationTimeout(TimeSpan.FromSeconds(30))
                           .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                           .Build();

                    Client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(Client_ApplicationMessageReceived);
                    Client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(x => OnConnected());
                    Client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(x => OnDisconnectedAsync());
                    try
                    {
                        Client.ConnectAsync(clientOptions);
                        initok = true;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError("MQTT CONNECTING FAILED", exception);
                    }
                    _logger.LogInformation("MQTT WAITING FOR APPLICATION MESSAGES");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("MQTT CONNECTING FAILED", exception);
            }
            return initok;
        }

        private async Task OnDisconnectedAsync()
        {
            try
            {
                await Client.ConnectAsync(clientOptions);
            }
            catch (Exception exception)
            {
                _logger.LogError("MQTT CONNECTING FAILED", exception);
            }
        }

        private void OnConnected()
        {
            Client.SubscribeAsync($"devices/+/rpc/request/+/+", MqttQualityOfServiceLevel.ExactlyOnce);
            Client.SubscribeAsync($"devices/Modbus/attributes/update/", MqttQualityOfServiceLevel.ExactlyOnce);
            Client.SubscribeAsync($"devices/+/attributes/response/+", MqttQualityOfServiceLevel.ExactlyOnce);
            _logger.LogInformation($"MQTT CONNECTED WITH SERVER ");
        }

        private Task Client_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogDebug($"ApplicationMessageReceived Topic {e.ApplicationMessage.Topic}  QualityOfServiceLevel:{e.ApplicationMessage.QualityOfServiceLevel} Retain:{e.ApplicationMessage.Retain} ");
            try
            {
                if (e.ApplicationMessage.Topic.StartsWith($"devices/") && e.ApplicationMessage.Topic.Contains("/response/"))
                {
                    ReceiveAttributes(e);
                }
                else if (e.ApplicationMessage.Topic.StartsWith($"devices/") && e.ApplicationMessage.Topic.Contains("/rpc/request/"))
                {
                    var tps = e.ApplicationMessage.Topic.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    var rpcmethodname = tps[4];
                    var rpcdevicename = tps[1];
                    var rpcrequestid = tps[5];
                    _logger.LogInformation($"rpcmethodname={rpcmethodname} ");
                    _logger.LogInformation($"rpcdevicename={rpcdevicename } ");
                    _logger.LogInformation($"rpcrequestid={rpcrequestid}   ");
                    if (!string.IsNullOrEmpty(rpcmethodname) && !string.IsNullOrEmpty(rpcdevicename) && !string.IsNullOrEmpty(rpcrequestid))
                    {
                        OnExcRpc?.Invoke(Client, new RpcRequest()
                        {
                            Method = rpcmethodname,
                            DeviceId = rpcdevicename,
                            RequestId = rpcrequestid,
                            Params = e.ApplicationMessage.ConvertPayloadToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClientId:{e.ClientId} Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}", ex);
            }
            return Task.CompletedTask;
        }

        private void ReceiveAttributes(MqttApplicationMessageReceivedEventArgs e)
        {
            var tps = e.ApplicationMessage.Topic.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var rpcmethodname = tps[2];
            var rpcdevicename = tps[1];
            var rpcrequestid = tps[4];
            _logger.LogInformation($"rpcmethodname={rpcmethodname} ");
            _logger.LogInformation($"rpcdevicename={rpcdevicename } ");
            _logger.LogInformation($"rpcrequestid={rpcrequestid}   ");

            if (!string.IsNullOrEmpty(rpcmethodname) && !string.IsNullOrEmpty(rpcdevicename) && !string.IsNullOrEmpty(rpcrequestid))
            {
                if (e.ApplicationMessage.Topic.Contains("/attributes/"))
                {
                    OnReceiveAttributes?.Invoke(Client, new AttributeResponse()
                    {
                        KeyName = rpcmethodname,
                        DeviceName = rpcdevicename,
                        Id = rpcrequestid,
                        Data = e.ApplicationMessage.ConvertPayloadToString()
                    });
                }
            }
        }

        public Task UploadAttributeAsync(string _devicename, object obj)
        {
            return Client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{_devicename}/attributes").WithPayload(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).Build());
        }

        public Task UploadTelemetryDataAsync(string _devicename, object obj)
        {
            return Client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{_devicename}/telemetry").WithPayload(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).Build());
        }

        public Task ResponseExecommand(RpcResponse rpcResult)
        {
            ///IoTSharp/Clients/RpcClient.cs#L65     var responseTopic = $"/devices/{deviceid}/rpc/response/{methodName}/{rpcid}";
            string topic = $"devices/{rpcResult.DeviceId}/rpc/response/{rpcResult.Method.ToString()}/{rpcResult.ResponseId}";
            return Client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(rpcResult.Data.ToString()).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        public Task RequestAttributes(string _device, bool anySide, params string[] args)
        {
            string id = Guid.NewGuid().ToString();
            string topic = $"devices/{_device}/attributes/request/{id}";
            Dictionary<string, string> keys = new Dictionary<string, string>();
            keys.Add(anySide ? "anySide" : "server", string.Join(",", args));
            Client.SubscribeAsync($"devices/{_device}/attributes/response/{id}", MqttQualityOfServiceLevel.ExactlyOnce);
            return Client.PublishAsync(topic, Newtonsoft.Json.JsonConvert.SerializeObject(keys), MqttQualityOfServiceLevel.ExactlyOnce);
        }

        public void PublishTelemetry(Device device, Dictionary<string, List<PayLoad>> SendModel)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        Client.PublishAsync("v1/gateway/telemetry", JsonConvert.SerializeObject(SendModel));
                        break;
                    case IoTPlatformType.IoTSharp:
                        foreach (var payload in SendModel[device.DeviceName])
                        {
                            UploadTelemetryDataAsync(device.DeviceName, payload.Values);
                        }
                        break;
                    case IoTPlatformType.AliCloudIoT:
                    case IoTPlatformType.TencentIoTHub:
                    case IoTPlatformType.BaiduIoTCore:
                    case IoTPlatformType.OneNET:
                    default:
                        break;
                }
                foreach (var payload in SendModel[device.DeviceName])
                {
                    foreach (var kv in payload.Values)
                    {
                        //更新到UAService
                        _uaNodeManager.UpdateNode($"{device.Parent.DeviceName}.{device.DeviceName}.{kv.Key}", kv.Value);
                    }
                }


            }
            catch (Exception ex)
            {

            }

        }

    }
    public class RpcRequest
    {
        public string DeviceId { get; set; }
        public string Method { get; set; }
        public string RequestId { get; set; }
        public string Params { get; set; }
    }
    public class RpcResponse
    {
        public string DeviceId { get; set; }
        public string Method { get; set; }
        public string ResponseId { get; set; }
        public string Data { get; set; }
    }
    public class AttributeResponse
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string KeyName { get; set; }

        public string Data { get; set; }
    }
}
