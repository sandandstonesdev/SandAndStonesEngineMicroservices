using Moq;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.Domain.Entities;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;
using SandAndStones.Infrastructure.Services.Textures;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class TextureSeederTests
    {
        private readonly Mock<ITextureRepository> _mockTextureRepository;
        private readonly Mock<IBitmapService> _mockBitmapService;
        private readonly TextureSeeder _textureSeeder;

        public TextureSeederTests()
        {
            _mockTextureRepository = new Mock<ITextureRepository>();
            _mockBitmapService = new Mock<IBitmapService>();
            _textureSeeder = new TextureSeeder(_mockBitmapService.Object, _mockTextureRepository.Object);
        }

        [Fact]
        public async Task StartAsync_ShouldSeedTextures_WhenBitmapsAndTexturesExist()
        {
            // Arrange
            var bitmaps = new List<Bitmap>
                {
                    new("test1.png", 256, 256, [1, 2, 3], "image/png"),
                    new("test2.png", 256, 256, [1, 2, 3], "image/png")
                };

            var textures = new List<Texture>
                {
                    new("test1.png", 256, 256, [1, 2, 3], "image/png"),
                    new("test2.png", 256, 256, [1, 2, 3], "image/png")
                };

            _mockBitmapService
                .Setup(service => service.EnumerateBitmaps(It.IsAny<string>()))
                .Returns(["test1.png", "test2.png"]);

            _mockBitmapService
                .Setup(service => service.Read(It.Is<string>(name => name == "test1.png")))
                .Returns(bitmaps[0]);

            _mockBitmapService
                .Setup(service => service.Read(It.Is<string>(name => name == "test2.png")))
                .Returns(bitmaps[1]);

            _mockTextureRepository
                .Setup(repo => repo.UploadTexture(It.Is<string>(name => name == "test1.png"), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(textures[0]);

            _mockTextureRepository
                .Setup(repo => repo.UploadTexture(It.Is<string>(name => name == "test2.png"), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(textures[1]);


            // Act
            await _textureSeeder.StartAsync(CancellationToken.None);

            // Assert
            _mockBitmapService.Verify(service => service.EnumerateBitmaps(It.IsAny<string>()), Times.Once);
            _mockBitmapService.Verify(service => service.Read(It.IsAny<string>()), Times.Exactly(textures.Count));
            _mockTextureRepository.Verify(repo => repo.UploadTexture(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()), Times.Exactly(textures.Count));
        }

        [Fact]
        public async Task StartAsync_FileLoadException_WhenBitmapsAndTexturesExist()
        {
            // Arrange
            var bitmaps = new List<Bitmap>
                {
                    new("test1.png", 256, 256, [1, 2, 3], "image/png"),
                    new("test2.png", 256, 256, [1, 2, 3], "image/png")
                };

            var failedTexture = new Texture("test1.png", 256, 256, new byte[] { 1, 2, 3 }, "image/png") { Loaded = false };

            _mockBitmapService
                .Setup(service => service.EnumerateBitmaps(It.IsAny<string>()))
                .Returns(["test1.png", "test2.png"]);

            _mockBitmapService
                .Setup(service => service.Read(It.Is<string>(name => name == "test1.png")))
                .Returns(bitmaps[0]);

            _mockBitmapService
                .Setup(service => service.Read(It.Is<string>(name => name == "test2.png")))
                .Returns(bitmaps[1]);

            _mockTextureRepository
                .Setup(repo => repo.UploadTexture(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(failedTexture);

            // Act & Assert
            await Assert.ThrowsAsync<FileLoadException>(() => _textureSeeder.StartAsync(CancellationToken.None));
        }
    }
}
