using System.Text.Json;

namespace SandAndStones.Shared.AssetConfig
{
    public class InputAssetReader(string fileName) : IAsyncAssetReader
    {
        private readonly string fileName = fileName;
        
        public async Task<InputAssetBatch> ReadBatchAsync()
        {
            using FileStream openStream = File.OpenRead(fileName);
            var inputAssetBatch =
                await JsonSerializer.DeserializeAsync<InputAssetBatch>(openStream,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            return inputAssetBatch;
        }
    }
}
