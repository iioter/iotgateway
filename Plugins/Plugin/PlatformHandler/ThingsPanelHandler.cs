using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using PluginInterface;

namespace Plugin.PlatformHandler
{
    public class ThingsPanelHandler : IPlatformHandler
    {
        public IManagedMqttClient MqttClient { get; }
        public ILogger<MessageService> Logger { get; }

        public event EventHandler<RpcRequest> OnExcRpc;
        private readonly DateTime _tsStartDt = new(1970, 1, 1);

        public ThingsPanelHandler(IManagedMqttClient mqttClient, ILogger<MessageService> logger, EventHandler<RpcRequest> onExcRpc)
        {
            MqttClient = mqttClient;
            Logger = logger;
            OnExcRpc += onExcRpc;
        }


        public async Task ClientConnected()
        {
            await MqttClient.SubscribeAsync("gateway/telemetry/control/+", MqttQualityOfServiceLevel.ExactlyOnce);
        }
        public void ReceiveRpc(MqttApplicationMessageReceivedEventArgs e)
        {
        }

        public async Task ResponseRpcAsync(RpcResponse rpcResponse)
        {
            await Task.CompletedTask;
        }

        public async Task PublishTelemetryAsync(string deviceName, Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            foreach (var payload in sendModel[deviceName])
            {
                if (payload.Values != null)
                {
                    var telemetryData = new Dictionary<string, Dictionary<string, object>>()
                    {
                        {
                            "sub_device_data", new Dictionary<string, object>()
                            {
                                { deviceName, payload.Values }
                            }
                        }

                    };
                    await MqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"gateway/telemetry")
                        .WithPayload(JsonConvert.SerializeObject(telemetryData)).Build());
                }
            }
        }

        public Task UploadAttributeAsync(string deviceName, object obj)
        {
            return Task.CompletedTask;
        }

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {
            await Task.CompletedTask;
        }

        public async Task DeviceConnected(string deviceName, Device device)
        {
            await Task.CompletedTask;
        }

        public async Task DeviceDisconnected(string deviceName, Device device)
        {

            await Task.CompletedTask;
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
