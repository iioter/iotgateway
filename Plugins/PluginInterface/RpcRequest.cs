namespace PluginInterface
{
    public class RpcRequest
    {
        public string DeviceName { get; set; }
        public string Method { get; set; }
        public string RequestId { get; set; }
        public Dictionary<string, object>? Params { get; set; }

        public override string ToString()
        {
            return $"Method:{Method},RequestId:{RequestId},Params:{Newtonsoft.Json.JsonConvert.SerializeObject(Params)}";
        }
    }
}