using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class SystemConfig : BasePoco
    {
        [Display(Name = "GatewayName")]
        public string GatewayName { get; set; }
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }
        [Display(Name = "MqttServer ")]
        public string MqttIp { get; set; }
        [Display(Name = "MqttPort")]
        public int MqttPort { get; set; }
        [Display(Name = "UserName")]
        public string MqttUName { get; set; }
        [Display(Name = "Password")]
        public string MqttUPwd { get; set; }
        [Display(Name = "OutputPlatform")]
        public IoTPlatformType IoTPlatformType { get; set; }
    }
    public enum IoTPlatformType
    {
        [Display(Name = "ThingsBoard")]
        ThingsBoard =0,
        [Display(Name = "IoTSharp")]
        IoTSharp =1,
        [Display(Name = "AliIoT")]
        AliCloudIoT=2,
        [Display(Name = "TencentIoTHub")]
        TencentIoTHub =3,
        [Display(Name = "BaiduIoTCore")]
        BaiduIoTCore =4,
        [Display(Name = "OneNet")]
        OneNET = 5,
        [Display(Name = "ThingsCloud")]
        ThingsCloud = 6,
        [Display(Name = "HuaWeiCloud")]
        HuaWei = 7,
        [Display(Name = "IoTGateway")]
        IoTGateway = 8
    }
}