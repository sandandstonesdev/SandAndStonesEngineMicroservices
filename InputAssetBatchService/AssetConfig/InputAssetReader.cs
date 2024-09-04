using System.Text.Json;

namespace InputAssetBatchService.AssetConfig
{
    public class InputAssetReader : IAsyncAssetReader
    {
        private readonly string fileName;
        public InputAssetReader(string fileName)
        {
            this.fileName = fileName;
        }

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
