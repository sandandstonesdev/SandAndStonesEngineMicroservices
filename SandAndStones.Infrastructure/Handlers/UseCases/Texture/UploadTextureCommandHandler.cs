using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Services.Bitmaps;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class UploadTextureCommandHandler(ITextureRepository inputTextureRepository, IBitmapService bitmapService) : IRequestHandler<UploadTextureCommand, UploadTextureCommandResponse>
    {
        private readonly ITextureRepository inputTextureRepository = inputTextureRepository;
        private readonly IBitmapService _bitmapService = bitmapService;
        public async Task<UploadTextureCommandResponse> Handle(UploadTextureCommand request, CancellationToken cancellationToken)
        {
            var rawData = _bitmapService.GetRawPixelsFromPng(request.Data);

            var result = await inputTextureRepository.UploadTexture(
                request.Name,
                request.Width,
                request.Height,
                rawData,
                MediaType.ImagePng
                );

            return new UploadTextureCommandResponse(result.Loaded);
        }
    }
}
