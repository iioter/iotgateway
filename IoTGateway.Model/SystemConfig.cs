using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class SystemConfig : BasePoco
    {
        [Display(Name = "网关名称")]
        public string GatewayName { get; set; }
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }
        [Display(Name = "Mqtt服务器")]
        public string MqttIp { get; set; }
        [Display(Name = "Mqtt端口")]
        public int MqttPort { get; set; }
        [Display(Name = "Mqtt用户名")]
        public string MqttUName { get; set; }
        [Display(Name = "Mqtt密码")]
        public string MqttUPwd { get; set; }
        [Display(Name = "输出平台")]
        public IoTPlatformType IoTPlatformType { get; set; }
    }
    public enum IoTPlatformType
    {
        [Display(Name = "ThingsBoard")]
        ThingsBoard =0,
        [Display(Name = "IoTSharp")]
        IoTSharp =1,
        [Display(Name = "阿里物联网平台")]
        AliCloudIoT=2,
        [Display(Name = "腾讯智能云")]
        TencentIoTHub =3,
        [Display(Name = "百度物联网通信")]
        BaiduIoTCore =4,
        [Display(Name = "中移OneNet")]
        OneNET = 5,
        [Display(Name = "ThingsCloud")]
        ThingsCloud = 6,
        [Display(Name = "华为云")]
        HuaWei = 7,
    }
}