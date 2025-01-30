using SandAndStones.Domain.Enums;
using SandAndStones.Infrastructure.Models.Assets;

namespace SandAndStones.Infrastructure.Services.Assets
{
    public interface IAsyncAssetReader
    {
        Task<InputAssetBatch> ReadBatchAsync(AssetBatchType assetBatchType);
    }
}
