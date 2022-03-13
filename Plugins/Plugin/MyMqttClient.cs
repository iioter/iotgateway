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
using Quickstarts.ReferenceServer;
using Opc.Ua;

namespace Plugin
{
    public class MyMqttClient//: IDependency
    {
        private IMqttClient _mqttClient = null;
        private ReferenceNodeManager _uaNodeManager = null;
        private MqttClientOptionsBuilder builder = null;
        private SystemConfig systemConfig = null;
        public MyMqttClient(UAService uaService)
        {
            _uaNodeManager = uaService.server.m_server.nodeManagers[0] as ReferenceNodeManager;
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
                    systemConfig = DC.Set<SystemConfig>().FirstOrDefault();
                    if (systemConfig == null)
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
                            .WithTcpServer(systemConfig.MqttIp, systemConfig.MqttPort)
                            .WithClientId(systemConfig.MqttUName + Guid.NewGuid().ToString())
                            .WithCredentials(systemConfig.MqttUName, systemConfig.MqttUPwd);

                        _mqttClient.ConnectAsync(builder.Build());
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void Publish(Device device, Dictionary<string, List<PayLoad>> SendModel)
        {
            try
            {
                switch (systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        _mqttClient.PublishAsync("v1/gateway/telemetry", JsonConvert.SerializeObject(SendModel));
                        break;
                    case IoTPlatformType.IoTSharp:
                        foreach (var payload in SendModel[device.DeviceName])
                        {
                            _mqttClient.PublishAsync($"devices/{device.DeviceName}/telemetry", JsonConvert.SerializeObject(payload.Values));
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
                        _uaNodeManager?.UpdateNode($"{device.Parent.DeviceName}.{device.DeviceName}.{kv.Key}", kv.Value);
                    }
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void Update2UAService()
        {
            int i = 0;
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
