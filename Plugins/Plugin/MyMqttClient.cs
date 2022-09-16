﻿using IoTGateway.DataAccess;
using IoTGateway.Model;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using PluginInterface;
using PluginInterface.HuaWeiRoma;
using PluginInterface.IotDB;
using PluginInterface.IoTSharp;
using PluginInterface.ThingsBoard;
using System.Xml.Linq;

namespace Plugin
{
    public class MyMqttClient
    {
        private readonly ILogger<MyMqttClient> _logger;
        //private readonly ReferenceNodeManager? _uaNodeManager;

        private SystemConfig _systemConfig;
        private ManagedMqttClientOptions _options;
        public bool IsConnected => (Client.IsConnected);
        private IManagedMqttClient? Client { get; set; }
        public event EventHandler<RpcRequest> OnExcRpc;
        public event EventHandler<ISAttributeResponse> OnReceiveAttributes;
        private readonly string _tbRpcTopic = "v1/gateway/rpc";

        //UAService uaService, 
        public MyMqttClient(ILogger<MyMqttClient> logger)
        {
            _logger = logger;
            //_uaNodeManager = uaService.server.m_server.nodeManagers[0] as ReferenceNodeManager;

            StartManagedClientAsync().Wait();
        }

        public async Task StartManagedClientAsync()
        {
            try
            {
                if (Client != null)
                {
                    Client.Dispose();
                }
                Client = new MqttFactory().CreateManagedMqttClient();
                await using var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType);
                _systemConfig = dc.Set<SystemConfig>().First();

                #region ClientOptions
                // Setup and start a managed MQTT client.
                _options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithMaxPendingMessages(10000)
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(string.IsNullOrWhiteSpace( _systemConfig.ClientId) ? Guid.NewGuid().ToString() : _systemConfig.ClientId)
                        .WithTcpServer(_systemConfig.MqttIp, _systemConfig.MqttPort)
                        .WithCredentials(_systemConfig.MqttUName, _systemConfig.MqttUPwd)
                        .WithTimeout(TimeSpan.FromSeconds(30))
                        .WithKeepAlivePeriod(TimeSpan.FromSeconds(20))
                        .Build())
                    .Build();
                #endregion
                #region Topics

                List<MqttTopicFilter> subTopics = new();
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        //{"device": "Device A", "data": {"id": $request_id, "method": "toggle_gpio", "params": {"pin":1}}}
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic(_tbRpcTopic).WithExactlyOnceQoS().Build());
                        //Message: {"id": $request_id, "device": "Device A", "value": "value1"}
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("v1/gateway/attributes/response").WithExactlyOnceQoS().Build());
                        //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("v1/gateway/attributese").WithExactlyOnceQoS().Build());
                        break;
                    case IoTPlatformType.IotDB:
                    case IoTPlatformType.IoTSharp:
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("devices/+/rpc/request/+/+").WithExactlyOnceQoS().Build());
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("devices/+/attributes/update").WithExactlyOnceQoS().Build());
                        //Message: {"device": "Device A", "data": {"attribute1": "value1", "attribute2": 42}}
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("devices/+/attributes/response/+").WithExactlyOnceQoS().Build());
                        break;
                    case IoTPlatformType.ThingsCloud:
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("gateway/attributes/response").WithExactlyOnceQoS().Build());
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("gateway/attributes/get/response").WithExactlyOnceQoS().Build());
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("gateway/attributes/push").WithExactlyOnceQoS().Build());
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("gateway/event/response").WithExactlyOnceQoS().Build());
                        subTopics.Add(new MqttTopicFilterBuilder().WithTopic("gateway/command/send").WithExactlyOnceQoS().Build());
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                }

                #endregion
                Client.ConnectedAsync += Client_ConnectedAsync;
                Client.DisconnectedAsync += Client_DisconnectedAsync;
                Client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
                await Client.SubscribeAsync(subTopics);
                await Client.StartAsync(_options);

                _logger.LogInformation("MQTT WAITING FOR APPLICATION MESSAGES");
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartManagedClientAsync FAILED, {ex}");
            }
        }

        private Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _logger.LogInformation($"MQTT CONNECTED WITH SERVER ");
            return Task.CompletedTask;
        }

        private Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogError($"MQTT CONNECTING FAILED, {arg.ReasonString}");
            return Task.CompletedTask;
        }
        
        private Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogDebug(
                $"ApplicationMessageReceived Topic {e.ApplicationMessage.Topic}  QualityOfServiceLevel:{e.ApplicationMessage.QualityOfServiceLevel} Retain:{e.ApplicationMessage.Retain} ");
            try
            {
                if (e.ApplicationMessage.Topic == _tbRpcTopic)
                    ReceiveTbRpc(e);
                else if (e.ApplicationMessage.Topic.StartsWith($"devices/") &&
                         e.ApplicationMessage.Topic.Contains("/response/"))
                {
                    ReceiveAttributes(e);
                }
                else if (e.ApplicationMessage.Topic.StartsWith($"devices/") &&
                         e.ApplicationMessage.Topic.Contains("/rpc/request/"))
                {
                    ReceiveIsRpc(e);
                }
                else if (e.ApplicationMessage.Topic == "gateway/command/send")
                {
                    ReceiveTcRpc(e);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"ClientId:{e.ClientId} Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}",
                    ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// thingsboard rpc
        /// </summary>
        /// <param name="e"></param>
        private void ReceiveTbRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tBRpcRequest =
                    JsonConvert.DeserializeObject<TBRpcRequest>(e.ApplicationMessage.ConvertPayloadToString());
                if (tBRpcRequest != null && !string.IsNullOrWhiteSpace(tBRpcRequest.RequestData.Method))
                {
                    OnExcRpc(Client, new RpcRequest()
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
                _logger.LogError(
                    $"ReceiveTbRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}",
                    ex);
            }
        }

        /// <summary>
        /// thingscloud rpc
        /// </summary>
        /// <param name="e"></param>
        private void ReceiveTcRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tCRpcRequest =
                    JsonConvert.DeserializeObject<TCRpcRequest>(e.ApplicationMessage.ConvertPayloadToString());
                if (tCRpcRequest != null)
                    OnExcRpc.Invoke(Client, new RpcRequest()
                    {
                        Method = tCRpcRequest.RequestData.Method,
                        DeviceName = tCRpcRequest.DeviceName,
                        RequestId = tCRpcRequest.RequestData.RequestId,
                        Params = tCRpcRequest.RequestData.Params
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"ReceiveTbRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}",
                    ex);
            }
        }

        private void ReceiveIsRpc(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var tps = e.ApplicationMessage.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var rpcMethodName = tps[4];
                var rpcDeviceName = tps[1];
                var rpcRequestId = tps[5];
                _logger.LogInformation($"rpcMethodName={rpcMethodName} ");
                _logger.LogInformation($"rpcDeviceName={rpcDeviceName} ");
                _logger.LogInformation($"rpcRequestId={rpcRequestId}   ");
                if (!string.IsNullOrEmpty(rpcMethodName) && !string.IsNullOrEmpty(rpcDeviceName) &&
                    !string.IsNullOrEmpty(rpcRequestId))
                {
                    Task.Run(() =>
                    {
                        OnExcRpc(Client, new RpcRequest()
                        {
                            Method = rpcMethodName,
                            DeviceName = rpcDeviceName,
                            RequestId = rpcRequestId,
                            Params = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.ApplicationMessage
                                .ConvertPayloadToString())
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"ReceiveIsRpc:Topic:{e.ApplicationMessage.Topic},Payload:{e.ApplicationMessage.ConvertPayloadToString()}",
                    ex);
            }
        }

        private async Task ResponseTbRpcAsync(TBRpcResponse tBRpcResponse)
        {
            await Client.EnqueueAsync(new MqttApplicationMessageBuilder()
                .WithTopic(_tbRpcTopic)
                .WithPayload(JsonConvert.SerializeObject(tBRpcResponse))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        private async Task ResponseTcRpcAsync(TCRpcRequest tCRpcResponse)
        {
            var topic = $"command/reply/{tCRpcResponse.RequestData.RequestId}";
            await Client.EnqueueAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(tCRpcResponse))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        private async Task ResponseIsRpcAsync(ISRpcResponse rpcResult)
        {
            //var responseTopic = $"/devices/{deviceid}/rpc/response/{methodName}/{rpcid}";
            var topic = $"devices/{rpcResult.DeviceId}/rpc/response/{rpcResult.Method}/{rpcResult.ResponseId}";
            await Client.EnqueueAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonConvert.SerializeObject(rpcResult))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).Build());
        }

        private void ReceiveAttributes(MqttApplicationMessageReceivedEventArgs e)
        {
            var tps = e.ApplicationMessage.Topic.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var rpcMethodName = tps[2];
            var rpcDeviceName = tps[1];
            var rpcRequestId = tps[4];
            _logger.LogInformation($"rpcMethodName={rpcMethodName}");
            _logger.LogInformation($"rpcDeviceName={rpcDeviceName}");
            _logger.LogInformation($"rpcRequestId={rpcRequestId}");

            if (!string.IsNullOrEmpty(rpcMethodName) && !string.IsNullOrEmpty(rpcDeviceName) &&
                !string.IsNullOrEmpty(rpcRequestId))
            {
                if (e.ApplicationMessage.Topic.Contains("/attributes/"))
                {
                    OnReceiveAttributes.Invoke(Client, new ISAttributeResponse()
                    {
                        KeyName = rpcMethodName,
                        DeviceName = rpcDeviceName,
                        Id = rpcRequestId,
                        Data = e.ApplicationMessage.ConvertPayloadToString()
                    });
                }
            }
        }

        public Task UploadAttributeAsync(string deviceName, object obj)
        {
            //Topic: v1/gateway/attributes
            //Message: {"Device A":{"attribute1":"value1", "attribute2": 42}, "Device B":{"attribute1":"value1", "attribute2": 42}
            try
            {
                if (Client.IsConnected)
                    return Client.EnqueueAsync(new MqttApplicationMessageBuilder()
                        .WithTopic($"devices/{deviceName}/attributes").WithPayload(JsonConvert.SerializeObject(obj))
                        .Build());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Device:{deviceName} UploadAttributeAsync Failed,{ex}");
            }

            return Task.CompletedTask;
        }

        public async Task UploadIsTelemetryDataAsync(string deviceName, object obj)
        {
            await Client.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic($"devices/{deviceName}/telemetry")
                .WithPayload(JsonConvert.SerializeObject(obj)).Build());
        }

        public async Task UploadTcTelemetryDataAsync(string deviceName, object obj)
        {
            var toSend = new Dictionary<string, object> { { deviceName, obj } };
            await Client.EnqueueAsync("gateway/attributes", JsonConvert.SerializeObject(toSend));
        }

        public async Task UploadHwTelemetryDataAsync(Device device, object obj)
        {
            var hwTelemetry = new List<HwTelemetry>()
            {
                new HwTelemetry()
                {
                    DeviceId = device.DeviceConfigs.FirstOrDefault(x => x.DeviceConfigName == "DeviceId")?.Value,
                    Services = new()
                    {
                        new Service()
                        {
                            ServiceId = "serviceId",
                            EventTime = DateTime.Now.ToString("yyyyMMddTHHmmssZ"),
                            Data = obj
                        }
                    }
                }
            };
            var hwTelemetrys = new HwTelemetrys()
            {
                Devices = hwTelemetry
            };

            await Client.EnqueueAsync($"/v1/devices/{_systemConfig.GatewayName}/datas",
                JsonConvert.SerializeObject(hwTelemetrys));
        }

        public async Task ResponseRpcAsync(RpcResponse rpcResponse)
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
                            ResponseData = new Dictionary<string, object>
                                { { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description } }
                        };
                        await ResponseTbRpcAsync(tRpcResponse);
                        break;
                    case IoTPlatformType.IoTSharp:
                        await ResponseIsRpcAsync(new ISRpcResponse
                        {
                            DeviceId = rpcResponse.DeviceName,
                            Method = "Method",
                            ResponseId = rpcResponse.RequestId,
                            Data = JsonConvert.SerializeObject(new Dictionary<string, object>
                            {
                                { "success", rpcResponse.IsSuccess }, { "description", rpcResponse.Description }
                            })
                        });
                        break;
                    case IoTPlatformType.ThingsCloud:
                        //官网API不需要回复的                        
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ResponseRpc Error,{rpcResponse}", ex);
            }
        }

        public async Task RequestAttributes(string deviceName, bool anySide, params string[] args)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                        //{"id": $request_id, "device": "Device A", "client": true, "key": "attribute1"}
                        Dictionary<string, object> tbRequestData = new Dictionary<string, object>
                        {
                            { "id", id },
                            { "device", deviceName },
                            { "client", true },
                            { "key", args[0] }
                        };
                        await Client.EnqueueAsync("v1/gateway/attributes/request",
                            JsonConvert.SerializeObject(tbRequestData), MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                    case IoTPlatformType.IoTSharp:
                        string topic = $"devices/{deviceName}/attributes/request/{id}";
                        Dictionary<string, string> keys = new Dictionary<string, string>();
                        keys.Add(anySide ? "anySide" : "server", string.Join(",", args));
                        await Client.SubscribeAsync($"devices/{deviceName}/attributes/response/{id}",
                            MqttQualityOfServiceLevel.ExactlyOnce);
                        await Client.EnqueueAsync(topic, JsonConvert.SerializeObject(keys),
                            MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"RequestAttributes:{deviceName}", ex);
            }
        }

        private Dictionary<string, List<PayLoad>> _lastTelemetrys = new(0);

        /// <summary>
        /// 判断是否推送遥测数据
        /// </summary>
        /// <param name="device">设备</param>
        /// <param name="sendModel">遥测</param>
        /// <returns></returns>
        private bool CanPubTelemetry(Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            bool canPub = false;
            try
            {
                //第一次上传
                if (!_lastTelemetrys.ContainsKey(device.DeviceName))
                    canPub = true;
                else
                {
                    //变化上传
                    if (device.CgUpload)
                    {
                        //是否超过归档周期
                        if (sendModel[device.DeviceName][0].TS - _lastTelemetrys[device.DeviceName][0].TS >
                            device.EnforcePeriod)
                            canPub = true;
                        //是否变化 这里不好先用
                        else
                        {
                            if (JsonConvert.SerializeObject(sendModel[device.DeviceName][0].Values) !=
                                JsonConvert.SerializeObject(_lastTelemetrys[device.DeviceName][0].Values))
                                canPub = true;
                        }
                    }
                    //非变化上传
                    else
                        canPub = true;
                }
            }
            catch (Exception e)
            {
                canPub = true;
                Console.WriteLine(e);
            }

            if (canPub)
                _lastTelemetrys[device.DeviceName] = sendModel[device.DeviceName];
            return canPub;
        }

        public async Task PublishTelemetryAsync(Device device, Dictionary<string, List<PayLoad>> sendModel)
        {
            try
            {
                if (CanPubTelemetry(device, sendModel))
                {
                    switch (_systemConfig.IoTPlatformType)
                    {
                        case IoTPlatformType.ThingsBoard:
                            await Client.EnqueueAsync("v1/gateway/telemetry",
                                JsonConvert.SerializeObject(sendModel));
                            break;
                        case IoTPlatformType.IoTSharp:
                            foreach (var payload in sendModel[device.DeviceName])
                            {
                                if (payload.Values != null)
                                    await UploadIsTelemetryDataAsync(device.DeviceName, payload.Values);
                            }

                            break;
                        case IoTPlatformType.ThingsCloud:
                            foreach (var payload in sendModel[device.DeviceName])
                            {
                                if (payload.Values != null)
                                    await UploadTcTelemetryDataAsync(device.DeviceName, payload.Values);
                            }

                            break;
                        case IoTPlatformType.IotDB:
                        {
                            foreach (var payload in sendModel[device.DeviceName])
                            {
                                if (payload.DeviceStatus != DeviceStatusTypeEnum.Good)
                                    continue;

                                IotTsData tsData = new IotTsData()
                                {
                                    device = _systemConfig.GatewayName + device.DeviceName,
                                    timestamp = payload.TS,
                                    measurements = payload.Values?.Keys.ToList(),
                                    values = payload.Values?.Values.ToList()
                                };
                                await Client.EnqueueAsync(_systemConfig.GatewayName + device.DeviceName, JsonConvert.SerializeObject(tsData));
                            }

                            break;
                        }
                        case IoTPlatformType.HuaWei:
                            foreach (var payload in sendModel[device.DeviceName])
                            {
                                if (payload.Values != null)
                                    await UploadHwTelemetryDataAsync(device, payload.Values);
                            }

                            break;

                        case IoTPlatformType.AliCloudIoT:
                        case IoTPlatformType.TencentIoTHub:
                        case IoTPlatformType.BaiduIoTCore:
                        case IoTPlatformType.OneNET:
                        default:
                            break;
                    }
                }

                //foreach (var payload in sendModel[device.DeviceName])
                //{
                //    if (payload.Values != null)
                //        foreach (var kv in payload.Values)
                //        {
                //            //更新到UAService
                //            _uaNodeManager?.UpdateNode($"{device.Parent.DeviceName}.{device.DeviceName}.{kv.Key}",
                //                kv.Value);
                //        }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"PublishTelemetryAsync Error:{ex}");
            }
        }

        private readonly DateTime _tsStartDt = new(1970, 1, 1);
        private readonly List<string> iotDbOnLineMeasurement = new() { "online" };
        private readonly List<object> iotDbOnLine = new() { true };
        private readonly List<object> iotDbOffLine = new() { false };
        public async Task DeviceConnected(Device device)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                    case IoTPlatformType.IoTSharp:
                        await Client.EnqueueAsync("v1/gateway/connect",
                            JsonConvert.SerializeObject(new Dictionary<string, string>
                                { { "device", device.DeviceName } }));
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                    case IoTPlatformType.ThingsCloud:
                        await Client.EnqueueAsync("gateway/connect",
                            JsonConvert.SerializeObject(new Dictionary<string, string>
                                { { "device", device.DeviceName } }));
                        break;
                    case IoTPlatformType.IotDB:
                        IotTsData onlineData = new IotTsData()
                        {
                            device = _systemConfig.GatewayName + device.DeviceName,
                            timestamp = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds,
                            measurements = iotDbOnLineMeasurement,
                            values = iotDbOnLine
                        };
                        await Client.EnqueueAsync(_systemConfig.GatewayName + device.DeviceName,
                            JsonConvert.SerializeObject(onlineData));
                        break;
                    case IoTPlatformType.HuaWei:
                        var deviceOnLine = new HwDeviceOnOffLine()
                        {
                            MId = new Random().Next(0, 1024), //命令ID
                            DeviceStatuses = new List<DeviceStatus>()
                            {
                                new DeviceStatus()
                                {
                                    DeviceId = device.DeviceConfigs
                                        .FirstOrDefault(x => x.DeviceConfigName == "DeviceId")
                                        ?.Value,
                                    Status = "ONLINE"
                                }
                            }
                        };
                        await Client.EnqueueAsync($"/v1/devices/{_systemConfig.GatewayName}/topo/update",
                            JsonConvert.SerializeObject(deviceOnLine), MqttQualityOfServiceLevel.AtLeastOnce,
                            retain: false);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceConnected:{device.DeviceName}", ex);
            }
        }

        public async Task DeviceDisconnected(Device device)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.ThingsBoard:
                    case IoTPlatformType.IoTSharp:
                        await Client.EnqueueAsync("v1/gateway/disconnect",
                            JsonConvert.SerializeObject(new Dictionary<string, string>
                                { { "device", device.DeviceName } }));
                        break;
                    case IoTPlatformType.AliCloudIoT:
                        break;
                    case IoTPlatformType.TencentIoTHub:
                        break;
                    case IoTPlatformType.BaiduIoTCore:
                        break;
                    case IoTPlatformType.OneNET:
                        break;
                    case IoTPlatformType.ThingsCloud:
                        await Client.EnqueueAsync("gateway/disconnect",
                            JsonConvert.SerializeObject(new Dictionary<string, string>
                                { { "device", device.DeviceName } }));
                        break;
                    case IoTPlatformType.IotDB:
                        IotTsData onlineData = new IotTsData()
                        {
                            device = _systemConfig.GatewayName + device.DeviceName,
                            timestamp = (long)(DateTime.UtcNow - _tsStartDt).TotalMilliseconds,
                            measurements = iotDbOnLineMeasurement,
                            values = iotDbOffLine
                        };
                        await Client.EnqueueAsync(_systemConfig.GatewayName + device.DeviceName,
                            JsonConvert.SerializeObject(onlineData));
                        break;
                    case IoTPlatformType.HuaWei:
                        var deviceOnLine = new HwDeviceOnOffLine()
                        {
                            MId = new Random().Next(0, 1024), //命令ID
                            DeviceStatuses = new List<DeviceStatus>()
                            {
                                new DeviceStatus()
                                {
                                    DeviceId = device.DeviceConfigs
                                        .FirstOrDefault(x => x.DeviceConfigName == "DeviceId")
                                        ?.Value,
                                    Status = "OFFLINE"
                                }
                            }
                        };
                        await Client.EnqueueAsync($"/v1/devices/{_systemConfig.GatewayName}/topo/update",
                            JsonConvert.SerializeObject(deviceOnLine), MqttQualityOfServiceLevel.AtLeastOnce,
                            retain: false);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceDisconnected:{device.DeviceName}", ex);
            }
        }

        public async Task DeviceAdded(Device device)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.HuaWei:
                        var topic = $"/v1/devices/{_systemConfig.GatewayName}/topo/add";

                        var addDeviceDto = new HwAddDeviceDto
                        {
                            MId = new Random().Next(0, 1024), //命令ID
                        };
                        addDeviceDto.DeviceInfos.Add(
                            new DeviceInfo
                            {
                                NodeId = device.DeviceName,
                                Name = device.DeviceName,
                                Description = device.Description,
                                ManufacturerId = "Test_n",
                                ProductType = "A_n"
                            }
                        );

                        await Client.EnqueueAsync(topic,
                            JsonConvert.SerializeObject(addDeviceDto));
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceAdded:{device.DeviceName}", ex);
            }
        }

        public async Task DeviceDeleted(Device device)
        {
            try
            {
                switch (_systemConfig.IoTPlatformType)
                {
                    case IoTPlatformType.HuaWei:
                        var topic = $"/v1/devices/{_systemConfig.GatewayName}/topo/delete";

                        await using (var dc = new DataContext(IoTBackgroundService.connnectSetting, IoTBackgroundService.DbType))
                        {
                            var deviceId = dc.Set<DeviceConfig>().FirstOrDefault(x =>
                                x.DeviceId == device.ID && x.DeviceConfigName == "DeviceId")?.Value;

                            var deleteDeviceDto = new HwDeleteDeviceDto
                            {
                                Id = new Random().Next(0, 1024), //命令ID
                                DeviceId = deviceId,
                                RequestTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds,
                                Request = new()
                                {
                                    ManufacturerId = "Test_n",
                                    ManufacturerName = "Test_n",
                                    ProductType = "A_n"
                                }
                            };

                            await Client.EnqueueAsync(topic,
                                JsonConvert.SerializeObject(deleteDeviceDto));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceAdded:{device.DeviceName}", ex);
            }
        }
    }
}