using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using PluginInterface;
using PluginInterface.IoTSharp;

namespace Plugin.PlatformHandler
{
    public class IoTSharpHandler : IPlatformHandler
    {
        public IManagedMqttClient MqttClient { get; }
        public ILogger<MessageService> Logger { get; }

        public event EventHandler<RpcRequest> OnExcRpc;

        private readonly DateTime _tsStartDt = new(1970, 1, 1);

        public IoTSharpHandler(IManagedMqttClient mqttClient, ILogger<MessageService> logger, EventHandler<RpcRequest> onExcRpc)
        {
            MqttClient = mqttClient;
            Logger = logger;
            OnExcRpc = onExcRpc;
        }

        public async Task ClientConnected()
        {
            await MqttClient.SubscribeAsync("devices/+/rpc/request/+/+", MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.SubscribeAsync("devices/+/attributes/update", MqttQualityOfServiceLevel.ExactlyOnce);
            //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
            await MqttClient.SubscribeAsync("devices/+/attributes/response/+", MqttQualityOfServiceLevel.ExactlyOnce);
        }

        public void ReceiveRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tps = e.ApplicationMessage.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var rpcMethodName = tps[4];
                var rpcDeviceName = tps[1];
                var rpcRequestId = tps[5];
                Logger.LogInformation($"rpcMethodName={rpcMethodName} ");
                Logger.LogInformation($"rpcDeviceName={rpcDeviceName} ");
                Logger.LogInformation($"rpcRequestId={rpcRequestId}   ");
                if (!string.IsNullOrEmpty(rpcMethodName) && !string.IsNullOrEmpty(rpcDeviceName) &&
                    !string.IsNullOrEmpty(rpcRequestId))
                {
                    Task.Run(() =>
                    {
                        var request = new RpcRequest()
                        {
                            Method = rpcMethodName,
                            DeviceName = rpcDeviceName,
                            RequestId = rpcRequestId,
                            Params = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.ApplicationMessage
                                .ConvertPayloadToString())
                        };
                        OnExcRpc?.Invoke(this, request);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,
                    $"ReceiveIsRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}");
            }
        }

        public async Task ResponseRpcAsync(RpcResponse rpcResponse)
        {
            var rpcResult = new ISRpcResponse
            {
                DeviceId = rpcResponse.DeviceName,
                Method = rpcResponse.Method,
                ResponseId = rpcResponse.RequestId,
                Data = JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description }
                })
            };

            //var responseTopic = $"/devices/{deviceid}/rpc/response/{methodName}/{rpcid}";
            var topic = $"devices/{rpcResult.DeviceId}/rpc/response/{rpcResult.Method}/{rpcResult.ResponseId}";

            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(JsonConvert.SerializeObject(rpcResult))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            foreach (var payload in sendModel[deviceName])
            {
                if (payload.Values != null)
                {
                    //payload.Values["_ts_"] = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds;
                    await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{deviceName}/telemetry")
                        .WithPayload(JsonConvert.SerializeObject(payload.Values)).Build());
                }
            }
        }

        public Task UploadAttributeAsync(string deviceName, object obj)
        {
            return Task.CompletedTask;
        }

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {
            string id = Guid.NewGuid().ToString();
            string topic = $"devices/{deviceName}/attributes/request/{id}";
            Dictionary<string, string> keys = new Dictionary<string, string>();
            keys.Add(anySide ? "anySide" : "server", string.Join(",", args));
            await MqttClient.SubscribeAsync($"devices/{deviceName}/attributes/response/{id}",
                MqttQualityOfServiceLevel.ExactlyOnce);
            await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(keys))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
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