using Newtonsoft.Json;

namespace PluginInterface.HuaWeiRoma
{
    public class HwDeleteDeviceDto
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        /// <summary>
        /// 平台生成的设备唯一标识，对应设备客户端ID
        /// </summary>
        [JsonProperty(PropertyName = "deviceId")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// 请求时间戳
        /// </summary>
        [JsonProperty(PropertyName = "requestTime")]
        public long RequestTime { get; set; }

        /// <summary>
        /// 子设备信息
        /// </summary>
        [JsonProperty(PropertyName = "request")]
        public Request? Request { get; set; }
    }

    public class Request
    {
        /// <summary>
        /// 厂商名称
        /// </summary>
        [JsonProperty(PropertyName = "manufacturerName")]
        public string? ManufacturerName { get; set; }

        /// <summary>
        /// 厂商ID
        /// </summary>
        [JsonProperty(PropertyName = "manufacturerId")]
        public string? ManufacturerId { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        [JsonProperty(PropertyName = "model")]
        public string? ProductType { get; set; }
    }
}
