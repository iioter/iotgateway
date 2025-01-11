using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
using PluginInterface;

namespace Plugin.PlatformHandler
{
    public static class PlatformHandlerFactory
    {
        public static IPlatformHandler CreateHandler(IoTPlatformType platform, IManagedMqttClient mqttClient, ILogger<MessageService> logger, EventHandler<RpcRequest> onExcRpc)
        {
            switch (platform)
            {
                case IoTPlatformType.ThingsBoard:
                    return new ThingsBoardHandler(mqttClient, logger, onExcRpc);
                case IoTPlatformType.ThingsCloud:
                    return new ThingsCloudHandler(mqttClient, logger, onExcRpc);
                case IoTPlatformType.IoTSharp:
                default:
                    return new IoTSharpHandler(mqttClient, logger, onExcRpc);
            }
        }
    }
}
