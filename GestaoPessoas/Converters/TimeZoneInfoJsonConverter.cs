using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeZoneConverter;

namespace GestaoPessoas.Converters
{
    public class TimeZoneInfoJsonConverter : JsonConverter<TimeZoneInfo>
    {
        public override TimeZoneInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeZoneId = reader.GetString();
            if (string.IsNullOrWhiteSpace(timeZoneId))
                return null;

            if (TZConvert.KnownIanaTimeZoneNames.Contains(timeZoneId))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            else
            {
                throw new JsonException($"TimeZone '{timeZoneId}' not found.");
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}