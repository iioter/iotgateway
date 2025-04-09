using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using PluginInterface;
using PluginInterface.ThingsBoard;

namespace Plugin.PlatformHandler
{
    public class ThingsCloudHandler : IPlatformHandler
    {
        public IManagedMqttClient MqttClient { get; }
        public ILogger<MessageService> Logger { get; }

        public event EventHandler<RpcRequest> OnExcRpc;

        public ThingsCloudHandler(IManagedMqttClient mqttClient, ILogger<MessageService> logger, EventHandler<RpcRequest> onExcRpc)
        {
            MqttClient = mqttClient;
            Logger = logger;
            OnExcRpc += onExcRpc;
        }

        public async Task ClientConnected()
        {
            await MqttClient.SubscribeAsync("gateway/attributes/response", MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.SubscribeAsync("gateway/attributes/get/response", MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.SubscribeAsync("gateway/attributes/push", MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.SubscribeAsync("gateway/event/response", MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.SubscribeAsync("gateway/command/send", MqttQualityOfServiceLevel.ExactlyOnce);
        }

        public void ReceiveRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tCRpcRequest =
                    JsonConvert.DeserializeObject<TCRpcRequest>(e.ApplicationMessage.ConvertPayloadToString());
                if (tCRpcRequest != null)
                {
                    Task.Run(() =>
                    {
                        var request = new RpcRequest()
                        {
                            Method = tCRpcRequest.RequestData.Method,
                            DeviceName = tCRpcRequest.DeviceName,
                            RequestId = tCRpcRequest.RequestData.RequestId,
                            Params = tCRpcRequest.RequestData.Params
                        };
                        OnExcRpc?.Invoke(this, request);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,
                    $"ReceiveTbRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}");
            }
        }

        public Task ResponseRpcAsync(RpcResponse rpcResponse)
        {
            return Task.CompletedTask;
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            foreach (var payload in sendModel[deviceName])
            {
                if (payload.Values != null)
                {
                    var toSend = new Dictionary<string, object> { { deviceName, payload.Values } };
                    await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"gateway/attributes")
                        .WithPayload(JsonConvert.SerializeObject(toSend)).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
                }
            }
        }

        public Task UploadAttributeAsync(string deviceName, object obj)
        {
            return Task.CompletedTask;
        }

        public Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {
            return Task.CompletedTask;
        }

        public async Task DeviceConnected(string deviceName, Device device)
        {
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic("gateway/connect")
                .WithPayload(JsonConvert.SerializeObject(new Dictionary<string, string>
                    { { "device", deviceName } }))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        public async Task DeviceDisconnected(string deviceName, Device device)
        {
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"gateway/disconnect")
                .WithPayload(JsonConvert.SerializeObject(new Dictionary<string, string>
                    { { "device", deviceName } }))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
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