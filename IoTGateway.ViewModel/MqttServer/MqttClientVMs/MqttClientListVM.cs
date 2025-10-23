﻿using MQTTnet.Formatter;
using MQTTnet.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.MqttClient.MqttServerVMs
{
    public partial class MqttClientListVM : BasePagedListVM<MqttClient_View, MqttClientSearcher>
    {
        protected override IEnumerable<IGridColumn<MqttClient_View>> InitGridHeader()
        {
            return new List<GridColumn<MqttClient_View>>{
                this.MakeGridHeader(x => x.ClientId),
                this.MakeGridHeader(x => x.Endpoint),
                this.MakeGridHeader(x => x.ReceivedApplicationMessagesCount),
                this.MakeGridHeader(x => x.SentApplicationMessagesCount),
                this.MakeGridHeader(x => x.ReceivedPacketsCount),
                this.MakeGridHeader(x => x.SentPacketsCount),
                this.MakeGridHeader(x => x.BytesSent),
                this.MakeGridHeader(x => x.BytesReceived),
                this.MakeGridHeader(x => x.PendingApplicationMessagesCount),
                this.MakeGridHeader(x => x.MqttProtocolVersion)
            };
        }

        protected override void InitListVM()
        {
            base.InitListVM();
        }

        protected override void InitVM()
        {
            base.InitVM();
        }

        public override void DoSearch()
        {
            var mqttServer = Wtm.ServiceProvider.GetService(typeof(MqttServer)) as MqttServer;
            var clients = mqttServer.GetClientsAsync().Result;
            foreach (var client in clients)
            {
                if (this.EntityList.Any(x => x.ClientId == client.Id))
                    continue;
                MqttClient_View mqttClient_ = new MqttClient_View
                {
                    ClientId = client.Id,
                    BytesReceived = client.BytesReceived,
                    BytesSent = client.BytesSent,
                    MqttProtocolVersion = client.ProtocolVersion,
                    ReceivedApplicationMessagesCount = client.ReceivedApplicationMessagesCount,
                    ReceivedPacketsCount = client.ReceivedPacketsCount,
                    SentApplicationMessagesCount = client.SentApplicationMessagesCount,
                    SentPacketsCount = client.SentPacketsCount,
                    PendingApplicationMessagesCount = client.Session.PendingApplicationMessagesCount
                };
                this.EntityList.Add(mqttClient_);
            }
        }
    }

    public class MqttClient_View : TopBasePoco
    {
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }

        [Display(Name = "Endpoint")]
        public string Endpoint { get; set; }

        [Display(Name = "RxMessages")]
        public long ReceivedApplicationMessagesCount { get; set; }

        [Display(Name = "TxMessages")]
        public long SentApplicationMessagesCount { get; set; }

        [Display(Name = "RxPackets")]
        public long ReceivedPacketsCount { get; set; }

        [Display(Name = "TxPackets")]
        public long SentPacketsCount { get; set; }

        [Display(Name = "TxBytes")]
        public long BytesSent { get; set; }

        [Display(Name = "RxBytes")]
        public long BytesReceived { get; set; }

        [Display(Name = "PendingMessage")]
        public long PendingApplicationMessagesCount { get; set; }

        [Display(Name = "ProtocolVersion")]
        public MqttProtocolVersion MqttProtocolVersion { get; set; }
    }
}