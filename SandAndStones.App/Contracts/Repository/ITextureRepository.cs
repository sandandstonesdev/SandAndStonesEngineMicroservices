using SandAndStones.Domain.Entities.Texture;

namespace SandAndStones.App.Contracts.Repository
{
    public interface ITextureRepository
    {
        Task<IEnumerable<string>> ListTexturesAsync();
        Task<Texture> DownloadTextureByName(string name, CancellationToken token);
        Task<Texture> UploadTexture(string name, int width, int height, byte[] data, string contentType);
    }
}
