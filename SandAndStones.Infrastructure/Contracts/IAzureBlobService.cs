using SandAndStones.Shared.TextureConfig;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface IAzureBlobService
    {
        Task<InputTexture> DownloadAsync(string fileName, CancellationToken token = default);
        Task<bool> DeleteAsync(string fileName);
        Task<Uri> UploadFileAsync(string fileName, Stream stream, CancellationToken token = default);
        
    }
}
