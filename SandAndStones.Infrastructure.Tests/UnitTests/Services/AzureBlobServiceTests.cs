using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Moq;
using SandAndStones.Infrastructure.Services.Blob;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;
using Azure;


namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class AzureBlobServiceTests
    {
        private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
        private readonly Mock<IBitmapService> _mockBitmapService;
        private readonly Mock<BlobContentInfo> _blobContentInfo;
        private readonly Mock<Response> _response;
        private readonly AzureBlobService _azureBlobService;


        public AzureBlobServiceTests()
        {
            _mockBlobServiceClient = new Mock<BlobServiceClient>();
            _mockBitmapService = new Mock<IBitmapService>();
            _azureBlobService = new AzureBlobService(_mockBlobServiceClient.Object, _mockBitmapService.Object);
            _blobContentInfo = new Mock<BlobContentInfo>();
            _response = new Mock<Response>();
        }


        [Fact]
        public async Task UploadFileAsync_ShouldReturnBlobUri()
        {
            // Arrange
            var bitmap = new Bitmap("test.png", 256, 256, [], "image/png");
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            mockBlobClient
                .Setup(x => x.Uri)
                .Returns(new Uri("https://test.blob.core.windows.net/test"));

            _mockBlobServiceClient
                .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(mockBlobContainerClient.Object);

            mockBlobContainerClient
                .Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(mockBlobClient.Object);

            mockBlobClient
                .Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobUploadOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(_blobContentInfo.Object, _response.Object));

            // Act
            var result = await _azureBlobService.UploadFileAsync(bitmap);

            // Assert
            Assert.NotNull(result);

            mockBlobClient.Verify(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobUploadOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DownloadFileAsync_ShouldReturnBlobUri()
        {
            // Arrange
            var filename = "test.png";
            var bitmap = new Bitmap(filename, 2, 2, [0, 1, 2, 3], "image /png");
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            var mockResponse = new Mock<Response<bool>>();
            mockResponse.Setup(x => x.Value).Returns(true);

            var mockBlobResponse = new Mock<Response>();
            
            var blobProperties = BlobsModelFactory.BlobProperties(contentType: "image/png", contentLength : 4);
            var blobPropertiesResponse = Response.FromValue(blobProperties, mockBlobResponse.Object);

            var memoryStream = new MemoryStream();
            var contentStream = new MemoryStream([ 1, 2, 3, 4 ]);

            var blobDownloadInfoMock = new Mock<Response<BlobDownloadInfo>>();
            
            mockBlobContainerClient
                .Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(mockBlobClient.Object);

            _mockBlobServiceClient
                .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(mockBlobContainerClient.Object);

            mockBlobClient
                .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockResponse.Object);

            mockBlobClient
                .Setup(x => x.GetPropertiesAsync(It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(blobPropertiesResponse);

            mockBlobClient
                .Setup(x => x.DownloadToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<Response>().Object);

            _mockBitmapService
                .Setup(service => service.ReadPngMetadata(It.IsAny<byte[]>()))
                .Returns((256, 256));

            _mockBitmapService
                .Setup(service => service.ValidatePngSignature(It.IsAny<byte[]>()))
                .Returns(true);

            // Act
            var result = await _azureBlobService.DownloadAsync(filename);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(filename, result.Name);
            Assert.Equal(256, result.Width);
            Assert.Equal(256, result.Height);
            Assert.Equal("image/png", result.ContentType);
            mockBlobClient.Verify(x => x.DownloadToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);;
        }
    }
}