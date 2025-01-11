using MQTTnet.Extensions.ManagedClient;
using IoTGateway.Model;
using PluginInterface;
using MQTTnet.Protocol;
using MQTTnet.Client;
using Microsoft.Extensions.Logging;
using MQTTnet;
using Newtonsoft.Json;
using PluginInterface.ThingsBoard;

namespace Plugin.PlatformHandler
{
    public class ThingsBoardHandler : IPlatformHandler
    {
        private readonly string _tbRpcTopic = "v1/gateway/rpc";

        public IManagedMqttClient MqttClient { get; }
        public ILogger<MessageService> Logger { get; }
        public event EventHandler<RpcRequest> OnExcRpc;

        public ThingsBoardHandler(IManagedMqttClient mqttClient, ILogger<MessageService> logger, EventHandler<RpcRequest> onExcRpc)
        {
            MqttClient = mqttClient;
            Logger = logger;
            OnExcRpc = onExcRpc;
        }

        public async Task ClientConnected()
        {
            //{"device": "Device A", "data": {"id": $request_id, "method": "toggle_gpio", "params": {"pin":1}}}
            await MqttClient.SubscribeAsync(_tbRpcTopic, MqttQualityOfServiceLevel.ExactlyOnce);
            //Message: {"id": $request_id, "device": "Device A", "value": "value1"}
            await MqttClient.SubscribeAsync("v1/gateway/attributes/response", MqttQualityOfServiceLevel.ExactlyOnce);
            //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
            await MqttClient.SubscribeAsync("v1/gateway/attributes", MqttQualityOfServiceLevel.ExactlyOnce);
        }

        public void ReceiveRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tBRpcRequest =
                    JsonConvert.DeserializeObject<TBRpcRequest>(e.ApplicationMessage.ConvertPayloadToString());
                if (tBRpcRequest != null && !string.IsNullOrWhiteSpace(tBRpcRequest.RequestData.Method))
                {
                    OnExcRpc(MqttClient, new RpcRequest()
                    {
                        Method = tBRpcRequest.RequestData.Method,
                        DeviceName = tBRpcRequest.DeviceName,
                        RequestId = tBRpcRequest.RequestData.RequestId,
                        Params = tBRpcRequest.RequestData.Params
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,
                    $"ReceiveTbRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}");
            }
        }

        public async Task ResponseRpcAsync(RpcResponse rpcResponse)
        {
            var tBRpcResponse = new TBRpcResponse
            {
                DeviceName = rpcResponse.DeviceName,
                RequestId = rpcResponse.RequestId,
                ResponseData = new Dictionary<string, object>
                    { { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description } }
            };

            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder()
                .WithTopic(_tbRpcTopic)
                .WithPayload(JsonConvert.SerializeObject(tBRpcResponse))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce).Build());
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic("v1/gateway/telemetry")
                .WithPayload(JsonConvert.SerializeObject(sendModel))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce).Build());
        }

        public Task UploadAttributeAsync(string deviceName, object obj)
        {
            return MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder()
                .WithTopic($"devices/{deviceName}/attributes").WithPayload(JsonConvert.SerializeObject(obj))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build());
        }

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {
            string id = Guid.NewGuid().ToString();
            //{"id": $request_id, "device": "Device A", "client": true, "key": "attribute1"}
            Dictionary<string, object> tbRequestData = new Dictionary<string, object>
            {
                { "id", id },
                { "device", deviceName },
                { "client", true },
                { "key", args[0] }
            };
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"v1/gateway/attributes/request")
                .WithPayload(JsonConvert.SerializeObject(tbRequestData)).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        public async Task DeviceConnected(string deviceName, Device device)
        {
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic("v1/gateway/connect")
                .WithPayload(JsonConvert.SerializeObject(new Dictionary<string, string>
                    { { "device", deviceName } }))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce).Build());
        }

        public async Task DeviceDisconnected(string deviceName, Device device)
        {
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"v1/gateway/disconnect")
                .WithPayload(JsonConvert.SerializeObject(new Dictionary<string, string>
                    { { "device", deviceName } }))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce).Build());
        }

        public Task DeviceAdded(Device device)
        {
            return Task.CompletedTask;
        }

        public Task DeviceDeleted(Device device)
        {
            return Task.CompletedTask;
        }
    }
}
