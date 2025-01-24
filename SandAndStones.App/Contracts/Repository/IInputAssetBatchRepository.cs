
using SandAndStones.App.Assets;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IInputAssetBatchRepository
    {
        Task<InputAssetBatch> GetById(int id);
    }
}
