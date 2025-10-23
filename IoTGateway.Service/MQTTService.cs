using System.Collections.Concurrent;
using MQTTnet.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace IoTGateway.Service
{
    public class MQTTService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactor;
        private readonly MqttServer _mqttServer;

        public ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> ClientTopics = new();

        public MQTTService(ILogger<MQTTService> logger, IServiceScopeFactory scopeFactor, MqttServer mqttServer)
        {
            _logger = logger;
            _scopeFactor = scopeFactor;
            _mqttServer = mqttServer;
        }

        public Task Server_Started(EventArgs e)
        {
            _logger.LogInformation($"MqttServer is  started");
            return Task.CompletedTask;
        }

        public Task Server_Stopped(EventArgs e)
        {
            _logger.LogInformation($"MqttServer is stopped");
            return Task.CompletedTask;
        }

        public Task Server_ClientConnectedAsync(ClientConnectedEventArgs e)
        {
            _logger.LogInformation($"Client [{e.ClientId}] {e.Endpoint} {e.UserName}  connected");
            if (e.ClientId.StartsWith("iotgatewayInternal_"))
            {
                ClientTopics.TryAdd(e.ClientId, new ConcurrentDictionary<string, byte>());
            }
            return Task.CompletedTask;
        }

        public Task Server_ClientDisconnected(ClientDisconnectedEventArgs e)
        {
            try
            {
                _logger.LogWarning($"Server_ClientDisconnected ClientId:{e.ClientId} DisconnectType:{e.DisconnectType}");
                if (e.ClientId.StartsWith("iotgatewayInternal_"))
                {
                    ClientTopics.TryRemove(e.ClientId, out _);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Server_ClientDisconnected ClientId:{e.ClientId} DisconnectType:{e.DisconnectType},{ex.Message}");
            }
            return Task.CompletedTask;
        }

        public Task Server_ClientSubscribedTopic(ClientSubscribedTopicEventArgs e)
        {
            _logger.LogInformation($"Client [{e.ClientId}] subscribed [{e.TopicFilter}]");
            if (e.ClientId.StartsWith("iotgatewayInternal_"))
            {
                // 获取或创建该 clientId 下的 Topic 集合
                var topics = ClientTopics.GetOrAdd(e.ClientId, _ => new ConcurrentDictionary<string, byte>());
                // 原子添加 topic
                topics.TryAdd(e.TopicFilter.Topic, 0);
            }
            return Task.CompletedTask;
        }

        public Task Server_ClientUnsubscribedTopic(ClientUnsubscribedTopicEventArgs e)
        {
            _logger.LogInformation($"Client [{e.ClientId}] unsubscribed[{e.TopicFilter}]");
            if (e.ClientId.StartsWith("iotgatewayInternal_"))
            {
                if (!ClientTopics.TryGetValue(e.ClientId, out var topics))
                    return Task.CompletedTask;

                // 原子删除 topic
                var removed = topics.TryRemove(e.TopicFilter, out _);

                // 如果该客户端再无订阅，可选地清理掉其条目
                if (removed && topics.IsEmpty)
                {
                    ClientTopics.TryRemove(e.ClientId, out _);
                }
            }
            return Task.CompletedTask;
        }

        public Task Server_ClientConnectionValidator(ValidatingConnectionEventArgs e)
        {
            //前端 websocket 连接通过客户端Id跳过验证
            if (e.ClientId.StartsWith("iotgatewayInternal_"))
                e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success;
            else
            {
                try
                {
                    //using var scope = _scopeFactor.CreateScope();
                    //using var wtm = scope.ServiceProvider.GetRequiredService<WTMContext>();

                    //var exist = wtm.DC.Set<FrameworkUser>().Any(x =>
                    //    x.ITCode.ToLower() == e.UserName.ToLower() &&
                    //    x.Password == Utils.GetMD5String(e.Password) && x.IsValid == true);
                    if (true)
                    {
                        e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success;
                    }
                    else
                    {
                        e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                        e.ReasonString = "BadUserNameOrPassword";
                        _logger.LogError("LoginError BadUserNameOrPassword");
                    }
                }
                catch (Exception ex)
                {
                    e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.ImplementationSpecificError;
                    e.ReasonString = ex.Message;
                    _logger.LogError(ex, "LoginError BadUserNameOrPassword");
                }
            }
            return Task.CompletedTask;
        }

        public Task DoRealTimeRefreshAsync(InjectedMqttApplicationMessage message)
        {
            return _mqttServer.InjectApplicationMessage(message);
        }

        public bool CanPub(string variableId)
        {
            if (ClientTopics.Any(x => x.Value.Keys.Any(y => y.EndsWith(variableId))))
                return true;
            return false;
        }
    }
}
