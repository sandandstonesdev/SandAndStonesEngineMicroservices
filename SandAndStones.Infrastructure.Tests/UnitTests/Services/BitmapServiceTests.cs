using Moq;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;
using System.IO.Abstractions;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class BitmapServiceTests
    {
        private readonly IBitmapService _bitmapService;
        private readonly Mock<IFileSystem> _fileSystem;
        private readonly Mock<IAppContextWrapper> _appContextWrapper;
        private readonly Mock<ISkiaWrapper> _skImageWrapper;
        public BitmapServiceTests()
        {
            _fileSystem = new Mock<IFileSystem>();
            _appContextWrapper = new Mock<IAppContextWrapper>();
            _skImageWrapper = new Mock<ISkiaWrapper>();
            _bitmapService = new BitmapService(
                _appContextWrapper.Object,
                _fileSystem.Object,
                _skImageWrapper.Object);
        }

        [Fact]
        public void EnumerateBitmaps_ShouldReturnBitmapList()
        {
            // Arrange
            var filename = "test.png";
            var path = "./Images";

            _fileSystem
                .Setup(fs => fs.Path.Combine(It.IsAny<string>()))
                .Returns(filename);
            _fileSystem
                .Setup(fs => fs.Directory.GetCurrentDirectory())
                .Returns(string.Empty);

            // Act
            var result = _bitmapService.EnumerateBitmaps(path);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Read_ShouldReturnBitmap()
        {
            // Arrange
            byte[] pngData = new byte[]
            {
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, // PNG signature
                0x00, 0x00, 0x00, 0x0D, // IHDR chunk length
                0x49, 0x48, 0x44, 0x52, // IHDR chunk type
                0x00, 0x00, 0x00, 0x02, // Width: 2 (00 00 00 02)
                0x00, 0x00, 0x00, 0x02, // Height: 2 (00 00 00 02)
                0x08, // Bit depth
                0x02, // Color type
                0x00, // Compression method
                0x00, // Filter method
                0x00, // Interlace method
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IDAT chunk (compressed image data)
                0x00, 0x00, 0x00, 0x0A, // IDAT chunk length
                0x49, 0x44, 0x41, 0x54, // IDAT chunk type
                0x78, 0x9C, // zlib header
                0x63, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, // Compressed rawData
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IEND chunk
                0x00, 0x00, 0x00, 0x00, // IEND chunk length
                0x49, 0x45, 0x4E, 0x44, // IEND chunk type
                0xAE, 0x42, 0x60, 0x82  // CRC
            };

            var rawData = new byte[] { 1, 2 };
            var width = 2;
            var height = 2;

            var filename = "test.png";
            var basePath = "/base/path";

            _appContextWrapper
                .SetupGet(wrapper => wrapper.BaseDirectory)
                .Returns(basePath);

            _fileSystem
                .Setup(fs => fs.Path.Combine(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(filename);

            _fileSystem
                .Setup(fs => fs.File.ReadAllBytes(It.IsAny<string>()))
                .Returns(pngData);

            _skImageWrapper
                .Setup(x => x.GetRawPixelsFromPng(pngData))
                .Returns((rawData, width, height));

            // Act
            var result = _bitmapService.Read(filename);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(filename, result.Name);
        }

        [Fact]
        public void Write_ShouldSaveBitmap()
        {
            // Arrange
            var filename = "test.png";
            var rawData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            var width = 2;
            var height = 2;
            var basePath = "/base/path";
            
            var bitmap = new Bitmap("test.png", width, height, rawData, "image/png");
            _appContextWrapper
                .SetupGet(wrapper => wrapper.BaseDirectory)
                .Returns(basePath);

            _fileSystem
                .Setup(fs => fs.Path.Combine(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(filename);

            _skImageWrapper
                .Setup(x => x.GetRawPixelsFromPng(rawData))
                .Returns((rawData, width, height));

            using var ms = new MemoryStream();
            ms.Write(rawData, 0, rawData.Length);
            ms.Position = 0;

            _fileSystem
                    .Setup(fs => fs.File.OpenWrite(It.IsAny<string>()))
                    .Returns(ms);

            // Act & Assert
            var exception = Record.Exception(() => _bitmapService.Write(bitmap));
            
            Assert.Null(exception);
        }

        [Fact]
        public void Delete_ShouldReturnTrue_WhenBitmapExists()
        {
            // Arrange
            var filename = "test.png";

            _fileSystem
                .Setup(fs => fs.File.Exists(It.IsAny<string>()))
                .Returns(true);
            _fileSystem
                .Setup(fs => fs.File.Delete(It.IsAny<string>()))
                .Verifiable();
            _fileSystem
                .Setup(fs => fs.Path.Combine(It.IsAny<string>()))
                .Returns(filename);

            // Act
            var result = _bitmapService.Delete(filename);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePngSignature_ShouldReturnTrue_WhenValidPng()
        {
            // Arrange
            var data = new byte[] { 0x89, (byte)'P', (byte)'N', (byte)'G' };

            // Act
            var result = _bitmapService.ValidatePngSignature(data);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetBytesAsPng_ShouldReturnPngData()
        {
            // Arrange
            byte[] pngData =
            [
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, // PNG signature
                0x00, 0x00, 0x00, 0x0D, // IHDR chunk length
                0x49, 0x48, 0x44, 0x52, // IHDR chunk type
                0x00, 0x00, 0x00, 0x02, // Width: 2 (00 00 00 02)
                0x00, 0x00, 0x00, 0x02, // Height: 2 (00 00 00 02)
                0x08, // Bit depth
                0x02, // Color type
                0x00, // Compression method
                0x00, // Filter method
                0x00, // Interlace method
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IDAT chunk (compressed image data)
                0x00, 0x00, 0x00, 0x0A, // IDAT chunk length
                0x49, 0x44, 0x41, 0x54, // IDAT chunk type
                0x78, 0x9C, // zlib header
                0x63, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, // Compressed rawData
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IEND chunk
                0x00, 0x00, 0x00, 0x00, // IEND chunk length
                0x49, 0x45, 0x4E, 0x44, // IEND chunk type
                0xAE, 0x42, 0x60, 0x82  // CRC
            ];

            var rawData = new byte[] { 1, 2 };
            var width = 2;
            var height = 2;

            _skImageWrapper
                .Setup(x => x.GetPngPixelsFromRaw(rawData, width, height))
                .Returns(pngData);

            // Act
            var result = _bitmapService.GetPngPixelsFromRaw(rawData, 2, 2);

            // Assert
            Assert.NotNull(result);
            
            Assert.True(result.Length > 0);

            Assert.Equal(0x89, result[0]);
            Assert.Equal((byte)'P', result[1]);
            Assert.Equal((byte)'N', result[2]);
            Assert.Equal((byte)'G', result[3]);
        }

        [Fact]
        public void ReadPngMetadata_ShouldReturnWidthAndHeight()
        {
            // Arrange
            byte[] pngData = [
                0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, // PNG signature
                0x00, 0x00, 0x00, 0x0D, // IHDR chunk length
                0x49, 0x48, 0x44, 0x52, // IHDR chunk type
                0x00, 0x00, 0x00, 0x02, // Width: 2 (00 00 00 02)
                0x00, 0x00, 0x00, 0x02, // Height: 2 (00 00 00 02)
                0x08, // Bit depth
                0x02, // Color type
                0x00, // Compression method
                0x00, // Filter method
                0x00, // Interlace method
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IDAT chunk (compressed image data)
                0x00, 0x00, 0x00, 0x0A, // IDAT chunk length
                0x49, 0x44, 0x41, 0x54, // IDAT chunk type
                0x78, 0x9C, // zlib header
                0x63, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, 0x60, // Compressed rawData
                0x00, 0x00, 0x00, 0x00, // CRC (placeholder)
                // IEND chunk
                0x00, 0x00, 0x00, 0x00, // IEND chunk length
                0x49, 0x45, 0x4E, 0x44, // IEND chunk type
                0xAE, 0x42, 0x60, 0x82  // CRC
            ];

            // Act
            var (width, height) = _bitmapService.ReadPngMetadata(pngData);

            // Assert
            Assert.Equal(2, width);
            Assert.Equal(2, height);
        }

        [Fact]
        public void GetTextureImageFilePath_ShouldReturnCorrectPath()
        {
            // Arrange
            var filename = "test.png";
            var path = "./Images";
            var basePath = "/base/path";

            _appContextWrapper
                .SetupGet(wrapper => wrapper.BaseDirectory)
                .Returns(basePath);

            _fileSystem
                .Setup(fs => fs.Path.Combine(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(path + filename);


            // Act
            var result = _bitmapService.GetTextureImageFilePath(filename);

            // Assert
            Assert.NotNull(result);

            Assert.Contains(filename, result);
        }
    }
}