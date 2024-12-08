namespace SandAndStones.App.UseCases.Texture.DownloadTextureByName
{
    public record DownloadTextureByNameQueryResponse(string FileName, byte[] FileData, string ContentType, bool Loaded);
}
