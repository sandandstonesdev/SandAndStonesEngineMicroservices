using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Shared.TextureConfig;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IInputTextureRepository
    {
        Task<bool> SeedTextures();
        List<TextureDescription> GetTextureDescriptionList();
        Task<DownloadTextureDto> DownloadTextureByName(string name);
        Task<InputTexture> GetById(int id);
        Task<UploadTextureDto> UploadTexture(string name, int width, int height, byte[] data);
    }
}
