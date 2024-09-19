namespace SandAndStones.Shared.AssetConfig
{
    public interface IAsyncAssetReader
    {
        Task<InputAssetBatch> ReadBatchAsync();
    }
}
