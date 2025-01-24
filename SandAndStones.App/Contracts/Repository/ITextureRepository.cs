using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Domain.Entities;

namespace SandAndStones.App.Contracts.Repository
{
    public interface ITextureRepository
    {
        Task<Texture> DownloadTextureByName(string name, CancellationToken token);
        Task<Texture> UploadTexture(string name, int width, int height, byte[] data, string contentType);
    }
}
