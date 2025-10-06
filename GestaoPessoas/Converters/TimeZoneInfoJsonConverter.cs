using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GestaoPessoas.Converters
{
    public class TimeZoneInfoJsonConverter : JsonConverter<TimeZoneInfo>
    {
        public override TimeZoneInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeZoneId = reader.GetString();
            if (string.IsNullOrWhiteSpace(timeZoneId))
                return TimeZoneInfo.Utc;
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.CreateCustomTimeZone(
                    id: "__INVALID__",
                    baseUtcOffset: TimeSpan.Zero,
                    displayName: "Invalid TimeZone",
                    standardDisplayName: "Invalid TimeZone"
                );
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}