namespace InputAssetBatchService.AssetConfig
{
    public interface IAsyncAssetReader
    {
        Task<InputAssetBatch> ReadBatchAsync();
    }
}
