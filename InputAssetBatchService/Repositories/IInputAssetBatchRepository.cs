using InputAssetBatchService.AssetConfig;

namespace InputAssetBatchService.Repositories
{
    public interface IInputAssetBatchRepository
    {
        Task<InputAssetBatch> GetById(int id);
    }
}
