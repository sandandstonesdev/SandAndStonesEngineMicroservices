using InputTextureService.TextureConfig;

namespace SandAndStones.Api
{
    public interface IInputTextureRepository
    {
        void Init();
        List<TextureDescription> GetTextureDescriptionList();
        Task<InputTexture> GetById(int id);
    }
}
