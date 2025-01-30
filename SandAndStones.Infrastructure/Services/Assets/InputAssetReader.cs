using SandAndStones.Domain.Enums;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Models.Assets;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.Infrastructure.Services.Assets
{
    public class InputAssetReader(
        BatchReaderConfig batchReaderConfig,
        IJsonSerializerService<InputAssetBatch> jsonSerializerService) : IAsyncAssetReader
    {
        private readonly IJsonSerializerService<InputAssetBatch> _jsonSerializerService = jsonSerializerService;
        
        public async Task<InputAssetBatch> ReadBatchAsync(AssetBatchType assetBatchType)
        {
            using FileStream openStream = File.OpenRead(
                batchReaderConfig.AssetPaths[assetBatchType]);
            var inputAssetBatch = _jsonSerializerService.Deserialize(openStream);

            return await Task.FromResult(inputAssetBatch);
        }
    }
}
