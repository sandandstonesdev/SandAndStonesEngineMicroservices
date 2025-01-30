using SandAndStones.Domain.Enums;
using System.Numerics;

namespace SandAndStones.Domain.Entities.Assets
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Instances { get; set; } = 1;
        public Vector4 StartPos { get; set; }
        public Vector4 EndPos { get; set; }
        public Vector4 InstancePosOffset { get; set; }
        public AssetBatchType AssetBatchType { get; set; }
        public AssetType AssetType { get; set; }
        public Vector4 Color { get; set; }
        public string Text { get; set; }
        public string[] AnimationTextureFiles { get; set; }
        public float Depth { get; set; }
        public float Scale { get; set; }

        public Asset(
            int id, string name, int instances,
            Vector4 startPos, Vector4 endPos,
            Vector4 instancePosOffset, 
            AssetBatchType assetBatchType, AssetType assetType, 
            Vector4 color, 
            string text, string[] animationTextureFiles, 
            float depth, float scale)
        {
            Id = id;
            Name = name;
            Instances = instances;
            StartPos = startPos;
            EndPos = endPos;
            InstancePosOffset = instancePosOffset;
            AssetBatchType = assetBatchType;
            AssetType = assetType;
            Color = color;
            Text = text;
            AnimationTextureFiles = animationTextureFiles;
            Depth = depth;
            Scale = scale;
        }
    }
}
