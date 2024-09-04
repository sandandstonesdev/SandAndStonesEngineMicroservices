using System.Numerics;
using System.Text.Json.Serialization;

namespace InputAssetBatchService.AssetConfig
{
    public class InputAsset
    {
        [JsonRequired]
        public int Id { get; set; }
        [JsonRequired]
        public string Name { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Instances { get; set; } = 1;
        [JsonConverter(typeof(JsonPosVectorConverter))]
        public Vector4 StartPos { get; set; }
        [JsonConverter(typeof(JsonPosVectorConverter))]
        public Vector4 EndPos { get; set; }
        [JsonConverter(typeof(JsonPosVectorConverter))]
        public Vector4 InstancePosOffset { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<AssetBatchType>))]
        public AssetBatchType AssetBatchType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<AssetType>))]
        public AssetType AssetType { get; set; }
        [JsonConverter(typeof(JsonColorVectorConverter))]
        public Vector4 Color { get; set; }
        public string Text { get; set; }
        public string[] AnimationTextureFiles { get; set; }
        public float Depth { get; set; }
        public float Scale { get; set; }
    }
}
