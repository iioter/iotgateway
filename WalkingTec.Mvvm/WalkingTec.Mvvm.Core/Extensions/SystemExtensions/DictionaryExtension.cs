using System.Collections.Generic;
using System.Text.Json;

namespace WalkingTec.Mvvm.Core.Extensions
{
    public static class DictionaryExtension
    {
        public static T GetValue<T>(this Dictionary<string, object> self, string key)
        {
            if (self.ContainsKey(key))
            {
                object v = self[key];
                if (v is JsonElement j)
                {
                    v = j.ToString();
                }
                var cv = v.ConvertValue(typeof(T));
                if (cv == null)
                {
                    return default(T);
                }
                else
                {
                    return (T)cv;
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}