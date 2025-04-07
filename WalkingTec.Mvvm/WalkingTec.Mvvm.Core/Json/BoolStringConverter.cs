using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WalkingTec.Mvvm.Core.Json
{
    public class BoolStringConverter :
JsonConverter<bool>
    {
        public override bool Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            JsonTokenType token = reader.TokenType;

            if (token == JsonTokenType.String)
            {
                var s = reader.GetString() ?? "";
                if (s.ToLower() == "true")
                {
                    return true;
                }
                else if (s.ToLower() == "false")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else if (token == JsonTokenType.True || token == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }
            else
            {
                return false;
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            bool data,
            JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(data);
        }
    }
}