using Moq;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.Infrastructure.Handlers.UseCases.Texture;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Domain.Entities.Texture;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Handlers
{

    public class GetTextureQueryHandlerTests
    {
        private readonly Mock<ITextureRepository> _mockTextureRepository;
        private readonly DownloadTextureByNameQueryHandler _handler;
        private readonly Mock<ITextureFileService> _mockTextureService;
        public GetTextureQueryHandlerTests()
        {
            _mockTextureRepository = new Mock<ITextureRepository>();
            _mockTextureService = new Mock<ITextureFileService>();
            _handler = new DownloadTextureByNameQueryHandler(_mockTextureRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTexture_WhenTextureExists()
        {
            // Arrange
            var textureName = "test.png";
            var texture = new Texture(textureName, 256, 256, [1, 2, 3], "image/png");

            _mockTextureRepository
                .Setup(repo => repo.DownloadTextureByName(textureName, CancellationToken.None))
                .ReturnsAsync(texture);

            var query = new DownloadTextureByNameQuery(textureName);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(textureName, result.FileName);
            Assert.Equal("image/png", result.ContentType);
            Assert.True(result.Loaded);
            Assert.NotEmpty(result.FileData);
            Assert.Equal(3, result.FileData.Length);

            _mockTextureRepository.Verify(repo => repo.DownloadTextureByName(textureName, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenTextureDoesNotExist()
        {
            // Arrange
            var textureName = "nonexistent.png";

            Texture texture = null!;
            _mockTextureRepository
                .Setup(service => service.DownloadTextureByName("nonexistent.png", CancellationToken.None))
                .ReturnsAsync(texture);

            var query = new DownloadTextureByNameQuery(textureName);

            // Act & Assert

            await Assert.ThrowsAsync<FileLoadException>(() => _handler.Handle(query, CancellationToken.None));

            _mockTextureRepository.Verify(repo => repo.DownloadTextureByName(textureName, CancellationToken.None), Times.Once);
        }
    }
}