namespace SandAndStones.App.UseCases.Texture.UploadTexture
{
    public record UploadTextureDto(string Name, byte[] Data, string ContentType, bool Loaded);
}
