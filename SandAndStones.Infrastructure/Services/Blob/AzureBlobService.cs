using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;

namespace SandAndStones.Infrastructure.Services.Blob
{
    public class AzureBlobService(BlobServiceClient blobServiceClient, IBitmapService bitmapService) : ITextureFileService
    {
        const string containerName = "textures";
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
        private readonly IBitmapService _bitmapService = bitmapService;

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

        public async Task<Bitmap> DownloadAsync(string fileName, CancellationToken token = default)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainer.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync(cancellationToken: token))
                throw new FileNotFoundException($"Cannot find blob: {fileName}");

            var properties = await blobClient.GetPropertiesAsync(cancellationToken: token);
            var contentType = properties.Value.ContentType;
            
            int attempts = 0;
            int tryCount = 3;

            using var downloadStream = new MemoryStream();
            while (attempts++ < tryCount)
            {
                try
                {
                    var response = await blobClient.DownloadToAsync(downloadStream, token);
                    if (response.IsError)
                        continue;

                    var imageData = downloadStream.ToArray();

                    var valid = _bitmapService.ValidatePngSignature(imageData);
                    if (!valid)
                        throw new InvalidDataException("Invalid PNG signature");
                    var (width, height) = _bitmapService.ReadPngMetadata(imageData);
                    if (width == 0 || height == 0)
                        throw new InvalidDataException("Invalid PNG metadata");

                    return new Bitmap(fileName, width, height, imageData, contentType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");
                    await Task.Delay(100, token);
                }
            }

            throw new Exception($"Failed to download blob: {fileName}");
        }

        public async Task<bool> DeleteAsync(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = blobContainer.GetBlobClient(fileName);
            bool result = await blobClient.DeleteIfExistsAsync();

            return result;
        }

        public async Task<Uri> UploadFileAsync(Bitmap bitmap, CancellationToken token = default)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

            var blob = blobContainer.GetBlobClient(bitmap.Name);

            var blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = MediaType.ImagePng }
            };

            var pngData = _bitmapService.GetPngPixelsFromRaw(bitmap.Data, bitmap.Width, bitmap.Height);
            using var ms = new MemoryStream(pngData);
            
            await blob.UploadAsync(ms, blobUploadOptions, cancellationToken: token);

            return  blob.Uri;
        }
    }
}
