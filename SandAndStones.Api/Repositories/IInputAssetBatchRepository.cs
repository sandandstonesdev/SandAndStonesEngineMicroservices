using SandAndStones.Shared.AssetConfig;

namespace SandAndStones.Api
{
    public interface IInputAssetBatchRepository
    {
        Task<InputAssetBatch> GetById(int id);
    }
}
