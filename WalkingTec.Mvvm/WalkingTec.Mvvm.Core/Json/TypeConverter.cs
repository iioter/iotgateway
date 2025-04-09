using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WalkingTec.Mvvm.Core.Json
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }
}