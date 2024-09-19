namespace SandAndStones.Shared.TextureConfig
{
    public interface IAsyncTextureReader
    {
        Task<InputTexture> ReadTextureAsync();
    }
}
