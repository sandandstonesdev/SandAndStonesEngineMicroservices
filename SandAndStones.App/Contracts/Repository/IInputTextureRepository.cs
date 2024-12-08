using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.Shared.TextureConfig;

namespace SandAndStones.App.Contracts.Repository
{
    public interface IInputTextureRepository
    {
        void Init();
        List<TextureDescription> GetTextureDescriptionList();
        Task<DownloadTextureDto> DownloadTextureByName(string name);
        Task<InputTexture> GetById(int id);
    }
}
