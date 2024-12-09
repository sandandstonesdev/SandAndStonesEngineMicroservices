using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.GetTextureById;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class GetTextureByIdQueryHandler(IInputTextureRepository inputTextureRepository) : IRequestHandler<GetTextureByIdQuery, GetTextureByIdQueryResponse>
    {
        private readonly IInputTextureRepository _inputTextureRepository = inputTextureRepository;

        public async Task<GetTextureByIdQueryResponse> Handle(GetTextureByIdQuery request, CancellationToken cancellationToken)
        {
            var texture = await _inputTextureRepository.GetById(request.Id);
            if (texture is null)
                return new GetTextureByIdQueryResponse(null);
            return new GetTextureByIdQueryResponse(texture);
        }
    }
}
