using MediatR;

namespace SandAndStones.App.UseCases.Texture.GetTextureById
{
    public record GetTextureByIdQuery(int Id) : IRequest<GetTextureByIdQueryResponse>;
}
