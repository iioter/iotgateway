using IoTGateway.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;

namespace IoTGateway.Extensions
{
    public static class MqttExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseMqttServer(this IApplicationBuilder app)
        {
            var mqttEvents = app.ApplicationServices.CreateScope().ServiceProvider.GetService<MQTTService>();
            app.UseMqttServer(server =>
            {
                server.StartedAsync += mqttEvents.Server_Started;
                server.StoppedAsync += mqttEvents.Server_Stopped;
                server.ClientConnectedAsync += mqttEvents.Server_ClientConnectedAsync;
                server.ValidatingConnectionAsync += mqttEvents.Server_ClientConnectionValidator;
                server.ClientSubscribedTopicAsync += mqttEvents.Server_ClientSubscribedTopic;
                server.ClientUnsubscribedTopicAsync += mqttEvents.Server_ClientUnsubscribedTopic;
                server.ClientDisconnectedAsync += mqttEvents.Server_ClientDisconnected;
            });
        }
    }
}
