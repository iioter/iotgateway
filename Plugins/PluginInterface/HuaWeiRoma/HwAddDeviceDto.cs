using Newtonsoft.Json;

namespace PluginInterface.HuaWeiRoma
{
    public class HwAddDeviceDto
    {
        [JsonProperty(PropertyName = "deviceInfos")]
        public List<DeviceInfo> DeviceInfos = new();

        /// <summary>
        /// 命令ID
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public long MId { get; set; }
    }

    public class DeviceInfo
    {
        /// <summary>
        /// 设备标识
        /// </summary>
        [JsonProperty(PropertyName = "nodeId")]
        public string NodeId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 厂商ID
        /// </summary>
        [JsonProperty(PropertyName = "manufacturerId")]
        public string ManufacturerId { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        [JsonProperty(PropertyName = "model")]
        public string ProductType { get; set; }
    }
}
