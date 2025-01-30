
using SandAndStones.Domain.Entities.Assets;
using SandAndStones.Domain.Enums;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IInputAssetBatchRepository
    {
        Task<AssetBatch> GetById(AssetBatchType assetBatchType);
    }
}
