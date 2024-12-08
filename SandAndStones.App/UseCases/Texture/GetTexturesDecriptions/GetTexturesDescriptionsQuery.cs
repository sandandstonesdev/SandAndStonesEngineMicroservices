using MediatR;

namespace SandAndStones.App.UseCases.Texture.GetTexturesDecriptions
{
    public record GetTexturesDescriptionsQuery : IRequest<GetTexturesDescriptionsQueryResponse>;
}
