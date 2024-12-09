using SandAndStones.Shared.TextureConfig;

namespace SandAndStones.App.UseCases.Texture.GetTexturesDecriptions
{
    public record GetTexturesDescriptionsQueryResponse(List<TextureDescription> TextureDescriptions);
}
