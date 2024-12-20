using MediatR;

namespace SandAndStones.App.UseCases.Texture.UploadTexture
{
    public record UploadTextureCommand(string Name, int Width, int Height, byte[] Data) : IRequest<UploadTextureCommandResponse>;
}
