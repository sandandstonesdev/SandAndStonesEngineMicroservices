using Moq;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.Contracts.Services;
using SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById;
using SandAndStones.Domain.Entities.Assets;
using SandAndStones.Domain.Enums;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Handlers.UseCases.AssetBatches;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.JsonSerialization;
using System.Numerics;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Handlers
{
    public class GetInputAssetBatchByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnInputAssetBatch_WhenInputAssetBatchExists()
        {
            // Arrange
            var repositoryMock = new Mock<IInputAssetBatchRepository>();
            var producerServiceMock = new Mock<IProducerService>();
            var tokenReaderServiceMock = new Mock<ITokenReaderService>();
            var assetBatch = new AssetBatch
            (
                1,
                [
                    new(
                        1,
                        "assetName",
                        1,
                        Vector4.One,
                        Vector4.One,
                        Vector4.One,
                        AssetBatchType.ClientRectBatch,
                        AssetType.Background,
                        Vector4.One,
                        "",
                        ["animationFile.png"],
                        1,
                        1
                    )
                ]
            );
            repositoryMock.Setup(x => x.GetById(It.IsAny<AssetBatchType>())).ReturnsAsync(assetBatch);
            var currentUserEmail = "currentUserEmail";
            tokenReaderServiceMock.Setup(x => x.GetUserEmailFromToken()).Returns(currentUserEmail);

            var serializer = JsonSerializerService<EventItem>.Create(
                JsonSerializerServiceOptions.EventItemOptions);

            var handler = new GetInputAssetBatchByIdQueryHandler(repositoryMock.Object, serializer, producerServiceMock.Object, tokenReaderServiceMock.Object);
            var query = new GetInputAssetBatchByIdQuery(0);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert

            tokenReaderServiceMock.Verify(x => x.GetUserEmailFromToken(), Times.Once);
            repositoryMock.Verify(x => x.GetById(It.IsAny<AssetBatchType>()), Times.Once);
            producerServiceMock.Verify(x => x.PublishMessageAsync(It.IsAny<string>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.Equal(assetBatch, result.InputAssetBatch);
        }
    }
}
