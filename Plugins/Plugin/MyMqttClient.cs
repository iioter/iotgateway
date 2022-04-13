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
using PluginInterface.IoTSharp;
using PluginInterface.ThingsBoard;
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
        public event EventHandler<ISAttributeResponse> OnReceiveAttributes;
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
            //v1/gateway/attributes
            //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}

            //v1/gateway/rpc
            //{"device": "Device A", "data": {"id": $request_id, "method": "toggle_gpio", "params": {"pin":1}}}
            Client.SubscribeAsync(tbRpcTopic, MqttQualityOfServiceLevel.ExactlyOnce);//tb server side rpc
            Client.SubscribeAsync($"devices/+/rpc/request/+/+", MqttQualityOfServiceLevel.ExactlyOnce);
            Client.SubscribeAsync($"devices/Modbus/attributes/update/", MqttQualityOfServiceLevel.ExactlyOnce);
            Client.SubscribeAsync($"devices/+/attributes/response/+", MqttQualityOfServiceLevel.ExactlyOnce);
            _logger.LogInformation($"MQTT CONNECTED WITH SERVER ");
        }

        private readonly string tbRpcTopic = @"v1/gateway/rpc";

        private Task Client_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogDebug($"ApplicationMessageReceived Topic {e.ApplicationMessage.Topic}  QualityOfServiceLevel:{e.ApplicationMessage.QualityOfServiceLevel} Retain:{e.ApplicationMessage.Retain} ");
            try
            {
                if (e.ApplicationMessage.Topic == tbRpcTopic)
                    ReceiveTbRpc(e);
                else if (e.ApplicationMessage.Topic.StartsWith($"devices/") && e.ApplicationMessage.Topic.Contains("/response/"))
                {
                    ReceiveAttributes(e);
                }
                else if (e.ApplicationMessage.Topic.StartsWith($"devices/") && e.ApplicationMessage.Topic.Contains("/rpc/request/"))
                {
                    ReceiveIsRpc(e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClientId:{e.ClientId} Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}", ex);
            }
            return Task.CompletedTask;
        }

        private void ReceiveTbRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            TBRpcRequest tBRpcRequest;
            try
            {
                tBRpcRequest = JsonConvert.DeserializeObject<TBRpcRequest>(e.ApplicationMessage.ConvertPayloadToString());
                OnExcRpc?.Invoke(Client, new RpcRequest()
                {
                    Method = tBRpcRequest.RequestData.Method,
                    DeviceName = tBRpcRequest.DeviceName,
                    RequestId = tBRpcRequest.RequestData.RequestId,
                    Params = tBRpcRequest.RequestData.Params
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReceiveTbRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}", ex);
            }

        }

        private void ReceiveIsRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
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
                        DeviceName = rpcdevicename,
                        RequestId = rpcrequestid,
                        Params = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.ApplicationMessage.ConvertPayloadToString())
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReceiveIsRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}", ex);

            }

        }

        private Task ResponseTBRpc(TBRpcResponse tBRpcResponse)
        {
            return Client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(tbRpcTopic)
                .WithPayload(JsonConvert.SerializeObject(tBRpcResponse))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        private Task ResponseISRpc(ISRpcResponse rpcResult)
        {
            ///IoTSharp/Clients/RpcClient.cs#L65     var responseTopic = $"/devices/{deviceid}/rpc/response/{methodName}/{rpcid}";
            string topic = $"devices/{rpcResult.DeviceId}/rpc/response/{rpcResult.Method}/{rpcResult.ResponseId}";
            return Client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(rpcResult))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
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
                    OnReceiveAttributes?.Invoke(Client, new ISAttributeResponse()
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
            //Topic: v1/gateway/attributes
            //Message: {"Device A":{"attribute1":"value1", "attribute2": 42}, "Device B":{"attribute1":"value1", "attribute2": 42}
            try
            {
                if (Client.IsConnected)
                    return Client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{_devicename}/attributes").WithPayload(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).Build());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Device:{_devicename} UploadAttributeAsync Failed,{ex}");
            }
            return Task.CompletedTask;

        }

        public Task UploadTelemetryDataAsync(string _devicename, object obj)
        {
            return Client.PublishAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{_devicename}/telemetry").WithPayload(Newtonsoft.Json.JsonConvert.SerializeObject(obj)).Build());
        }

        public void ResponseRpc(RpcResponse rpcResponse)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        var tRpcResponse = new TBRpcResponse
                        {
                            DeviceName = rpcResponse.DeviceName,
                            RequestId = rpcResponse.RequestId,
                            ResponseData = new Dictionary<string, object> { { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description } }
                        };
                        ResponseTBRpc(tRpcResponse);
                        break;
                    case IoTPlatformType.IoTSharp:
                        ResponseISRpc(new ISRpcResponse
                        {
                            DeviceId = rpcResponse.DeviceName,
                            Method = "Method",
                            ResponseId = rpcResponse.RequestId,
                            Data = JsonConvert.SerializeObject(new Dictionary<string, object> { { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description } })
                        });
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ResponseRpc Error,{rpcResponse}", ex);
            }

        }

        public Task RequestAttributes(string _device, bool anySide, params string[] args)
        {
            //Topic: v1/gateway/attributes/request
            // Message: { "id": $request_id, "device": "Device A", "client": true, "key": "attribute1"}
            //Topic: v1/gateway/attributes/response
            //Message: {"id": $request_id, "device": "Device A", "value": "value1"}
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

        public void DeviceConnected()
        {
            //Topic: v1/gateway/connect
            //Message: { "device":"Device A"}
        }

        public void DeviceDisconnected()
        {
            //Topic: v1/gateway/connect
            //Message: { "device":"Device A"}

        }

    }
}
