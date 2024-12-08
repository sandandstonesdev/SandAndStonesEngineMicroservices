using MediatR;

namespace SandAndStones.App.UseCases.Texture.DownloadTextureByName
{
    public record DownloadTextureByNameQuery(string Name) : IRequest<DownloadTextureByNameQueryResponse>;
}
