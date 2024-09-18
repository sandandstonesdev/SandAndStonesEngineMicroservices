using SandAndStonesLibrary.AssetConfig;

namespace SandAndStones.Api
{
    public interface IInputAssetBatchRepository
    {
        Task<InputAssetBatch> GetById(int id);
    }
}
