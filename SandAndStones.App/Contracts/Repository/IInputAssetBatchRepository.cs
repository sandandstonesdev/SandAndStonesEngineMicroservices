using SandAndStones.Shared.AssetConfig;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IInputAssetBatchRepository
    {
        Task<InputAssetBatch> GetById(int id);
    }
}
