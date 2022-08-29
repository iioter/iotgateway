using Newtonsoft.Json;

namespace PluginInterface.HuaWeiRoma
{
    public class HwTelemetrys
    {
        [JsonProperty(PropertyName = "devices")]
        public List<HwTelemetry>? Devices { get; set; }
    }
    public class HwTelemetry
    {
        [JsonProperty(PropertyName = "deviceId")]
        public string? DeviceId { get; set; }
        [JsonProperty(PropertyName = "services")]
        public List<Service>? Services { get; set; }
    }

    public class Service
    {
        [JsonProperty(PropertyName = "serviceId")]
        public string? ServiceId { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object? Data { get; set; }
        [JsonProperty(PropertyName = "eventTime")]
        public string? EventTime { get; set; }
    }
}
