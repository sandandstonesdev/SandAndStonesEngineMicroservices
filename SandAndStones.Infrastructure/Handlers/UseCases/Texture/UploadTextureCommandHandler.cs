using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.UploadTexture;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class UploadTextureCommandHandler(IInputTextureRepository inputTextureRepository) : IRequestHandler<UploadTextureCommand, UploadTextureCommandResponse>
    {
        private readonly IInputTextureRepository inputTextureRepository = inputTextureRepository;
        public async Task<UploadTextureCommandResponse> Handle(UploadTextureCommand request, CancellationToken cancellationToken)
        {
            var result = await inputTextureRepository.UploadTexture(
                request.Name,
                request.Width,
                request.Height,
                request.Data);

            return new UploadTextureCommandResponse(result.Loaded);
        }
    }
}
