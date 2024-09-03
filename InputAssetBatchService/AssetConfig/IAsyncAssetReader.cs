namespace SandAndStonesEngine.Assets.AssetConfig
{
    public interface IAsyncAssetReader
    {
        Task<InputAssetBatch> ReadBatchAsync();
    }
}
