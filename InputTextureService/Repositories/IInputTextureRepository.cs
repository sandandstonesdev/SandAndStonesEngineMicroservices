using InputTextureService.TextureConfig;

namespace InputTextureService.Repositories
{
    public interface IInputTextureRepository
    {
        void Init();
        List<TextureDescription> GetTextureDescriptionList();
        Task<InputTexture> GetById(int id);
    }
}
