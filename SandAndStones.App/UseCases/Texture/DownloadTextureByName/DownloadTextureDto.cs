namespace SandAndStones.App.UseCases.Texture.DownloadTextureByName
{
    public record DownloadTextureDto(string Name, byte[] Data, string ContentType, bool Loaded);
}
