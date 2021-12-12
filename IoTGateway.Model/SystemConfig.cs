using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class SystemConfig : BasePoco
    {
        [Display(Name = "网关名称")]
        public string GatewayName { get; set; }
        [Display(Name = "Mqtt服务器")]
        public string MqttIp { get; set; }
        [Display(Name = "Mqtt端口")]
        public int MqttPort { get; set; }
        [Display(Name = "Mqtt用户名")]
        public string MqttUName { get; set; }
        [Display(Name = "Mqtt密码")]
        public string MqttUPwd { get; set; }
    }
}