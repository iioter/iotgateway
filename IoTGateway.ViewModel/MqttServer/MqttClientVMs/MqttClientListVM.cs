using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;
using Microsoft.Extensions.Primitives;
using MQTTnet.Server;
using MQTTnet.Formatter;

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
        public override void AfterDoSearcher()
        {
        }
        public override void DoSearch()
        {
            var mqttServer = Wtm.ServiceProvider.GetService(typeof(MqttServer)) as MqttServer;
            foreach (var client in mqttServer.GetClientsAsync().Result)
            {
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
            int i = 0;
        }
    }
    public class MqttClient_View : TopBasePoco
    {
        [Display(Name = "客户端Id")]
        public string ClientId { get; set; }

        [Display(Name = "Endpoint")]
        public string Endpoint { get; set; }

        [Display(Name = "收消息数")]
        public long ReceivedApplicationMessagesCount { get; set; }

        [Display(Name = "收发消息数")]
        public long SentApplicationMessagesCount { get; set; }

        [Display(Name = "收包数")]
        public long ReceivedPacketsCount { get; set; }

        [Display(Name = "发包数")]
        public long SentPacketsCount { get; set; }

        [Display(Name = "发字节数")]
        public long BytesSent { get; set; }

        [Display(Name = "收字节数")]
        public long BytesReceived { get; set; }

        [Display(Name = "未决消息数")]
        public long PendingApplicationMessagesCount { get; set; }

        [Display(Name = "协议版本")]
        public MqttProtocolVersion MqttProtocolVersion { get; set; }
    }

}
