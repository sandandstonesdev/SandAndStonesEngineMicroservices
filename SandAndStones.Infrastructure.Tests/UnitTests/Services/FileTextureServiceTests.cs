using Moq;
using SandAndStones.Infrastructure.Services.Blob;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class FileTextureServiceTests
    {
        private readonly Mock<IBitmapService> _mockBitmapService;
        private readonly FileTextureService _fileTextureService;

        public FileTextureServiceTests()
        {
            _mockBitmapService = new Mock<IBitmapService>();
            _fileTextureService = new FileTextureService(_mockBitmapService.Object);
        }

        [Fact]
        public async Task DownloadAsync_ShouldReturnBitmap()
        {
            // Arrange
            var fileName = "test.png";
            var bitmap = new Bitmap(fileName, 256, 256, [], "image/png");

            _mockBitmapService
                .Setup(service => service.Read(fileName))
                .Returns(bitmap);

            _mockBitmapService
                .Setup(service => service.GetRawPixelsFromPng(bitmap.Data))
                .Returns(bitmap.Data);

            // Act
            var result = await _fileTextureService.DownloadAsync(fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileName, result.Name);

            _mockBitmapService.Verify(service => service.Read(fileName), Times.Once);
            _mockBitmapService.Verify(service => service.GetRawPixelsFromPng(bitmap.Data), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenBitmapExists()
        {
            // Arrange
            var fileName = "test.png";

            _mockBitmapService
                .Setup(service => service.Delete(fileName))
                .Returns(true);

            // Act
            var result = await _fileTextureService.DeleteAsync(fileName);

            // Assert
            Assert.True(result);

            _mockBitmapService.Verify(service => service.Delete(fileName), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenBitmapDoesNotExist()
        {
            // Arrange
            var fileName = "nonexistent.png";

            _mockBitmapService
                .Setup(service => service.Delete(fileName))
                .Returns(false);

            // Act
            var result = await _fileTextureService.DeleteAsync(fileName);

            // Assert
            Assert.False(result);

            _mockBitmapService.Verify(service => service.Delete(fileName), Times.Once);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldReturnUri()
        {
            // Arrange
            var bitmap = new Bitmap("test.png", 256, 256, [], "image/png");
            var expectedUri = new Uri("file://path/to/test.png");

            _mockBitmapService
                .Setup(service => service.Write(bitmap));

            _mockBitmapService
                .Setup(service => service.GetTextureImageFilePath(bitmap.Name, "Images"))
                .Returns(expectedUri.ToString());

            // Act
            var result = await _fileTextureService.UploadFileAsync(bitmap);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUri, result);

            _mockBitmapService.Verify(service => service.Write(bitmap), Times.Once);
            _mockBitmapService.Verify(service => service.GetTextureImageFilePath(bitmap.Name, "Images"), Times.Once);
        }
    }
}