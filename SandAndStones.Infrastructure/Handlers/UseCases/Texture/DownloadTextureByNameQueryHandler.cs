using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class DownloadTextureByNameQueryHandler(ITextureRepository inputTextureRepository) : IRequestHandler<DownloadTextureByNameQuery, DownloadTextureByNameQueryResponse>
    {
        private readonly ITextureRepository _inputTextureRepository = inputTextureRepository;

        public async Task<DownloadTextureByNameQueryResponse> Handle(DownloadTextureByNameQuery request, CancellationToken cancellationToken)
        {
            var downloadedTexture = await _inputTextureRepository.DownloadTextureByName(request.Name, cancellationToken);

            if (downloadedTexture is null)
            {
                throw new FileLoadException("Cannot download texture.");
            }

            return new DownloadTextureByNameQueryResponse(
                downloadedTexture.Name,
                downloadedTexture.Data,
                downloadedTexture.ContentType,
                downloadedTexture.Loaded);
        }
    }
}
