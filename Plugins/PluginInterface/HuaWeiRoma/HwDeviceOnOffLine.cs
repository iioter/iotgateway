using Newtonsoft.Json;

namespace PluginInterface.HuaWeiRoma
{
    public class HwDeviceOnOffLine
    {
        [JsonProperty(PropertyName = "deviceStatuses")]
        public List<DeviceStatus> DeviceStatuses = new();

        /// <summary>
        /// 命令ID
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public long MId { get; set; }
    }

    public class DeviceStatus
    {
        /// <summary>
        /// 设备标识
        /// </summary>
        [JsonProperty(PropertyName = "deviceId")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// 子设备状态：
        /// OFFLINE：设备离线
        /// ONLINE： 设备上线
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string? Status { get; set; }

    }
}
