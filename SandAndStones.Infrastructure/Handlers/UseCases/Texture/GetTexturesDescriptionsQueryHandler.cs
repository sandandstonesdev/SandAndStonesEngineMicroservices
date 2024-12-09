using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.GetTexturesDecriptions;

namespace SandAndStones.Infrastructure.Handlers.UseCases.Texture
{
    public class GetTexturesDescriptionsQueryHandler(IInputTextureRepository inputTextureRepository) : IRequestHandler<GetTexturesDescriptionsQuery, GetTexturesDescriptionsQueryResponse>
    {
        private readonly IInputTextureRepository _inputTextureRepository = inputTextureRepository;

        public async Task<GetTexturesDescriptionsQueryResponse> Handle(GetTexturesDescriptionsQuery request, CancellationToken cancellationToken)
        {
            var textureDescriptionList = _inputTextureRepository.GetTextureDescriptionList();
            if (textureDescriptionList is null)
                return new GetTexturesDescriptionsQueryResponse([]);
            return new GetTexturesDescriptionsQueryResponse(textureDescriptionList);
        }
    }
}
