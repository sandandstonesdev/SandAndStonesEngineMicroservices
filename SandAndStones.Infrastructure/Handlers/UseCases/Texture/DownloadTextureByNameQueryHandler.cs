using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class DownloadTextureByNameQueryHandler(IInputTextureRepository inputTextureRepository) : IRequestHandler<DownloadTextureByNameQuery, DownloadTextureByNameQueryResponse>
    {
        private readonly IInputTextureRepository _inputTextureRepository = inputTextureRepository;
        public async Task<DownloadTextureByNameQueryResponse> Handle(DownloadTextureByNameQuery request, CancellationToken cancellationToken)
        {
            var downloadedTexture = await _inputTextureRepository.DownloadTextureByName(request.Name);
            return new DownloadTextureByNameQueryResponse(
                downloadedTexture.Name,
                downloadedTexture.Data,
                downloadedTexture.ContentType,
                downloadedTexture.Loaded);
        }
    }
}
