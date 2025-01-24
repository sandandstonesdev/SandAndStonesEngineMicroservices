using Moq;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Repositories;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class TextureRepositoryTests
    {
        private readonly Mock<ITextureFileService> _mockTextureFileService;
        private readonly TextureRepository _textureRepository;

        public TextureRepositoryTests()
        {
            _mockTextureFileService = new Mock<ITextureFileService>();
            _textureRepository = new TextureRepository(_mockTextureFileService.Object);
        }

        [Fact]
        public async Task DownloadTextureByName_ShouldReturnTexture_WhenTextureExists()
        {
            // Arrange
            var textureName = "test.png";
            var bitmap = new Bitmap(textureName, 256, 256, [1, 2, 3], "image/png");

            _mockTextureFileService
                .Setup(service => service.DownloadAsync(textureName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bitmap);

            // Act
            var result = await _textureRepository.DownloadTextureByName(textureName, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(textureName, result.Name);
            _mockTextureFileService.Verify(service => service.DownloadAsync(textureName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DownloadTextureByName_ShouldThrowFileLoadException_WhenTextureDoesNotExist()
        {
            // Arrange
            var textureName = "nonexistent.png";

            Bitmap bitmap = null!;
            
            _mockTextureFileService
                .Setup(service => service.DownloadAsync(textureName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bitmap);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _textureRepository.DownloadTextureByName(textureName, CancellationToken.None));
            _mockTextureFileService.Verify(service => service.DownloadAsync(textureName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UploadTexture_ShouldReturnUploadTextureDto()
        {
            // Arrange
            var textureName = "test.png";
            var width = 256;
            var height = 256;
            byte[] data = [1, 2, 3];
            var contentType = "image/png";
            var bitmap = new Bitmap(textureName, width, height, data, contentType);
            var uri = new Uri("http://example.com/test.png");

            _mockTextureFileService
                .Setup(service => service.UploadFileAsync(It.IsAny<Bitmap>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(uri);

            // Act
            var result = await _textureRepository.UploadTexture(textureName, width, height, data, contentType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(textureName, result.Name);
            Assert.Equal(width, result.Width);
            Assert.Equal(height, result.Height);
            Assert.Equal(data, result.Data);
            Assert.Equal(contentType, result.ContentType);
            _mockTextureFileService.Verify(service => service.UploadFileAsync(It.IsAny<Bitmap>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
