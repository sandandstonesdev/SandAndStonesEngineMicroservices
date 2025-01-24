using System.Numerics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SandAndStones.App.Assets
{
    public class JsonColorVectorConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<List<float>>(ref reader, options);
            _ = list ?? throw new Exception();
            return new Vector4(list[0], list[1], list[2], list[3]);
        }


        public override void Write(
            Utf8JsonWriter writer,
            Vector4 vectorValue,
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(vectorValue.X);
            writer.WriteNumberValue(vectorValue.Y);
            writer.WriteNumberValue(vectorValue.Z);
            writer.WriteNumberValue(vectorValue.W);
            writer.WriteEndArray();
        }
    }

}
