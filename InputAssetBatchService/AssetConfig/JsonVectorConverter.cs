using System.Globalization;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SandAndStonesEngine.Assets.AssetConfig
{
    public class JsonAssetBatchConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<List<float>>(ref reader, options);
            _ = list ?? throw new Exception();
            return new Vector4(list[0], list[1], 
                            list.Count > 2 ? list[2] : 0,
                            list.Count > 3 ? list[3] : 0);
        }


        public override void Write(
            Utf8JsonWriter writer,
            Vector4 vectorValue,
            JsonSerializerOptions options)
        {

            writer.WriteStringValue($"[{vectorValue.X}, {vectorValue.Y}, {vectorValue.Z}]");
        }
    }
}