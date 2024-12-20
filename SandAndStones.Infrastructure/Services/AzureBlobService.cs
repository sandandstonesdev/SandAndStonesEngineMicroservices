using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Shared.TextureConfig;

namespace SandAndStones.Infrastructure.Services
{
    public class AzureBlobService(BlobServiceClient blobServiceClient) : IAzureBlobService
    {
        const string containerName = "textures";
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

        public async Task ListBlobContainerAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();

            await foreach (var container in containers)
            {
                Console.WriteLine(container.Name);
            }
        }

        public async Task<IAsyncEnumerable<BlobItem>> ListBlobsAsync()
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = container.GetBlobsAsync();

            await foreach (var blob in blobs)
            {
                Console.WriteLine("Blob name: {0}", blob.Name);
            }

            return blobs;
        }

        public async Task<InputTexture> DownloadAsync(string fileName, CancellationToken token = default)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = blobContainer.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var data = await blobClient.OpenReadAsync(cancellationToken: token);
                Stream blobContent = data;

                var content = await blobClient.DownloadContentAsync(token);

                string contentType = content.Value.Details.ContentType;

                return new InputTexture(fileName, contentType, blobContent);
            }

            return new InputTexture();
        }
        
        public async Task<bool> DeleteAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = blobContainer.GetBlobClient(fileName);
            bool result = await blobClient.DeleteIfExistsAsync();

            return result;
        }

        public async Task<Uri> UploadFileAsync(string fileName, Stream stream, CancellationToken token = default)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

            var blob = blobContainer.GetBlobClient(fileName);
            
            await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = "image/png" });
            
            return blob.Uri;
        }
    }
}
