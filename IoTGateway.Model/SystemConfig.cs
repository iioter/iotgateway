using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class SystemConfig : BasePoco
    {
        [Display(Name = "Gateway Name")]
        public string GatewayName { get; set; }
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }
        [Display(Name = "Mqtt Server ")]
        public string MqttIp { get; set; }
        [Display(Name = "Mqtt Port")]
        public int MqttPort { get; set; }
        [Display(Name = "Mqtt UserName")]
        public string MqttUName { get; set; }
        [Display(Name = "Mqtt Password")]
        public string MqttUPwd { get; set; }
        [Display(Name = "Output platform")]
        public IoTPlatformType IoTPlatformType { get; set; }
    }
    public enum IoTPlatformType
    {
        [Display(Name = "ThingsBoard")]
        ThingsBoard =0,
        [Display(Name = "IoTSharp")]
        IoTSharp =1,
        [Display(Name = "Alibaba Internet of Things Platform")]
        AliCloudIoT=2,
        [Display(Name = "Tencent Intelligent Cloud")]
        TencentIoTHub =3,
        [Display(Name = "Baidu Internet of Things Communication")]
        BaiduIoTCore =4,
        [Display(Name = "中移OneNet")]
        OneNET = 5,
        [Display(Name = "ThingsCloud")]
        ThingsCloud = 6,
        [Display(Name = "Huawei Cloud")]
        HuaWei = 7,
        [Display(Name = "IoTGateway")]
        IoTGateway = 8
    }
}