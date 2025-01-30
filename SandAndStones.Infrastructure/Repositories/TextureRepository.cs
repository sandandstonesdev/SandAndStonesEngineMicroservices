using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Domain.Entities.Texture;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Repositories
{
    public class TextureRepository(ITextureFileService textureService) : ITextureRepository
    {
        private readonly ITextureFileService _textureService = textureService;

        public async Task<Texture> DownloadTextureByName(string name, CancellationToken cancellationToken)
        {
            var bitmap = await _textureService.DownloadAsync(name, cancellationToken);

            return 
                new Texture(
                bitmap.Name,
                bitmap.Width,
                bitmap.Height,
                bitmap.Data,
                bitmap.ContentType);
        }

        public async Task<Texture> UploadTexture(string name, int width, int height, byte[] data, string contentType)
        {
            var bitmap = new Bitmap(name, width, height, data, contentType);
            await _textureService.UploadFileAsync(bitmap);
            
            return new Texture(
                bitmap.Name,
                bitmap.Width,
                bitmap.Height,
                bitmap.Data,
                bitmap.ContentType);
        }
    }
}
