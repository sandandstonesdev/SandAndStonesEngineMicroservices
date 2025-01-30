using System.Text.Json.Serialization;

namespace SandAndStones.Infrastructure.Services.JsonSerialization
{
    public sealed class JsonDateTimeFormatAttribute : JsonConverterAttribute
    {
        private readonly string format;

        public JsonDateTimeFormatAttribute(string format) => this.format = format;

        public string Format => format;

        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            return new CustomDateTimeFormatConverter(format);
        }
    }
}