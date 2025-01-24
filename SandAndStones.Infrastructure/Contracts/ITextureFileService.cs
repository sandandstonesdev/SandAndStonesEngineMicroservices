using SandAndStones.Domain.Entities;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface ITextureFileService
    {
        Task<Bitmap> DownloadAsync(string fileName, CancellationToken token = default);
        Task<bool> DeleteAsync(string fileName);
        Task<Uri> UploadFileAsync(Bitmap bitmap, CancellationToken token = default);
        
    }
}
