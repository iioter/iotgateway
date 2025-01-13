using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("传输配置")]
    public class SystemConfig : BasePoco
    {
        [Comment("网关名称")]
        [Display(Name = "GatewayName")]
        public string GatewayName { get; set; }

        [Comment("ClientId")]
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }

        [Comment("Mqtt服务器")]
        [Display(Name = "MqttServer ")]
        public string MqttIp { get; set; }

        [Comment("Mqtt端口")]
        [Display(Name = "MqttPort")]
        public int MqttPort { get; set; }

        [Comment("Mqtt用户名")]
        [Display(Name = "UserName")]
        public string MqttUName { get; set; }

        [Comment("Mqtt密码")]
        [Display(Name = "Password")]
        public string MqttUPwd { get; set; }

        [Comment("输出平台")]
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
        IoTGateway = 8,
        [Display(Name = "ThingsPanel")]
        ThingsPanel = 9
    }
}