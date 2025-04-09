using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Plugin
{
    public static class JsonUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="data">要序列化的数据</param>
        /// <param name="dateTimeFormat">时间格式 默认yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static string SerializeToJson(object data, string dateTimeFormat = "HH:mm:ss.fff")
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter()
            {
                DateTimeFormat = dateTimeFormat
            };
            return JsonConvert.SerializeObject(data, Formatting.Indented, timeConverter);
        }
    }
}