using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using System;
using Newtonsoft.Json;
using WalkingTec.Mvvm.Core;
using System.Collections.Generic;
using IoTGateway.DataAccess;
using IoTGateway.Model;
using System.Linq;
using PluginInterface;
using Microsoft.Extensions.DependencyInjection;

namespace Plugin
{
    public class MyMqttClient//: IDependency
    {
        private static IMqttClient _mqttClient = null;
        private static MqttClientOptionsBuilder builder = null;
        public MyMqttClient()
        {
            InitClient();
        }
        public void InitClient()
        {
            try
            {
                if (_mqttClient != null)
                    _mqttClient.Dispose();

                using (var DC = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DBType))
                {
                    var systemManage = DC.Set<SystemConfig>().FirstOrDefault();
                    if (systemManage == null)
                        Console.WriteLine("配置信息错误，无法启动");
                    else
                    {
                        _mqttClient = new MqttFactory().CreateMqttClient();
                        _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(x => OnConnected());
                        _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(x => OnDisconnected());
                        _mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnReceived);
                        builder = new MqttClientOptionsBuilder()
                            .WithCommunicationTimeout(TimeSpan.FromSeconds(60))
                            .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                            .WithTcpServer(systemManage.MqttIp, systemManage.MqttPort)
                            .WithClientId(systemManage.MqttUName + Guid.NewGuid().ToString())
                            .WithCredentials(systemManage.MqttUName, systemManage.MqttUPwd);

                        _mqttClient.ConnectAsync(builder.Build());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public void Publish(string Topic, Dictionary<string, List<PayLoad>> SendModel)
        {
            _mqttClient.PublishAsync(Topic, JsonConvert.SerializeObject(SendModel));
        }

        private void OnReceived(MqttApplicationMessageReceivedEventArgs obj)
        {
            var topic = obj.ApplicationMessage.Topic;
            var msg = System.Text.Encoding.UTF8.GetString(obj.ApplicationMessage.Payload);
            Console.WriteLine($"{topic}: {msg}");
        }

        private void OnDisconnected()
        {
            Console.WriteLine("Mqtt连接断开");
            _mqttClient.ConnectAsync(builder.Build());
        }

        private void OnConnected()
        {
            Console.WriteLine("Mqtt连接正常");
        }
    }
}
