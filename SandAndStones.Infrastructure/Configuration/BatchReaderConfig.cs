using SandAndStones.Domain.Enums;
namespace SandAndStones.Infrastructure.Configuration
{
    public class BatchReaderConfig
    {
        public Dictionary<AssetBatchType, string> AssetPaths { get; set; } = new Dictionary<AssetBatchType, string>
        {
            { AssetBatchType.ClientRectBatch, "./Assets/assets.json" },
            { AssetBatchType.StatusBarBatch, "./Assets/statusBarAssets.json" }
        };
    }
}
