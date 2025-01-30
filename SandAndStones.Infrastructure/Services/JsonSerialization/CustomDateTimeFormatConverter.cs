using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

public class CustomDateTimeFormatConverter : JsonConverter<DateTime>
{
    private readonly string format;

    public CustomDateTimeFormatConverter(string format)
    {
        this.format = format;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? stringToDeserialize = reader.GetString();
        ArgumentNullException.ThrowIfNull(stringToDeserialize, nameof(stringToDeserialize));

        return DateTime.ParseExact(
            stringToDeserialize,
            format,
            CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer, nameof(writer));

        writer.WriteStringValue(value
            .ToUniversalTime()
            .ToString(
                format,
                CultureInfo.InvariantCulture));
    }
}
