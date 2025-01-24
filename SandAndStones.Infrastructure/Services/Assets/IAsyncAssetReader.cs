using SandAndStones.App.Assets;

namespace SandAndStones.Infrastructure.Services.Asset
{
    public interface IAsyncAssetReader
    {
        Task<InputAssetBatch> ReadBatchAsync();
    }
}
