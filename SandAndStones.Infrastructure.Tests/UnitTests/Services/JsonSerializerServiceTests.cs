using Moq;
using SandAndStones.Domain.Enums;
using SandAndStones.Infrastructure.Models.Assets;
using SandAndStones.Infrastructure.Services.Datetime;
using SandAndStones.Infrastructure.Services.JsonSerialization;
using System.Numerics;
using System.Text;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class JsonSerializerServiceTests
    {
        private readonly DateTime dateTimeNow = new DateTime(2025, 1, 30, 5, 55, 55);
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public JsonSerializerServiceTests()
        {
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _dateTimeProviderMock.Setup(x => x.Now).Returns(
                dateTimeNow.ToUniversalTime());
            _dateTimeProviderMock.Setup(x => x.NowString).Returns(
                dateTimeNow.ToUniversalTime().ToString(dateTimeFormat));
        }

        [Fact]
        public void JsonSerializer_ShouldSerializeAsset_WhenAssetType()
        {
            var serializer = JsonSerializerService<InputAsset>.Create(
                JsonSerializerServiceOptions.AssetOptions);

            var inputAsset = new InputAsset
            {
                Id = 1,
                Name = "TestAsset",
                StartPos = new Vector4(1, 2, 3, 4),
                EndPos = new Vector4(5, 6, 7, 8),
                Instances = 2,
                InstancePosOffset = new Vector4(9, 10, 11, 12),
                AnimationTextureFiles = new string[] { "file1", "file2" },
                AssetBatchType = AssetBatchType.ClientRectBatch,
                AssetType = AssetType.Background,
                Color = new Vector4(13, 14, 15, 16),
                Depth = 17,
                Scale = 18,
                Text = "TestText"
            };

            string serializedAsset = serializer.Serialize(inputAsset);

            string expectedJson = @"
            {
                ""id"": 1,
                ""name"": ""TestAsset"",
                ""instances"": 2,
                ""startPos"": [1, 2],
                ""endPos"": [5, 6],
                ""instancePosOffset"": [9, 10],
                ""assetBatchType"": ""ClientRectBatch"",
                ""assetType"": ""Background"",      
                ""color"": [13, 14, 15, 16],
                ""text"": ""TestText"",
                ""animationTextureFiles"": [""file1"", ""file2""],
                ""depth"": 17,
                ""scale"": 18
            }";

            Assert.Equal(expectedJson.Replace(" ", "").Replace("\n", "").Replace("\r", ""), serializedAsset.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        public void JsonSerializer_ShouldSerializeEventGrid_WhenEventGridType()
        {
            var serializer = JsonSerializerService<EventItem>.Create(
                JsonSerializerServiceOptions.EventItemOptions);

            var eventItem = new EventItem
            {
                Id = "0",
                ResourceId = 0,
                ResourceName = "Asset",
                CurrentUserId = "test@email.com",
                DateTime = _dateTimeProviderMock.Object.Now
            };

            string serializedEventGrid = serializer.Serialize(eventItem);

            string expectedJson = $@"
            {{
                ""id"": ""0"",
                ""resourceId"": 0,
                ""resourceName"": ""Asset"",
                ""dateTime"": ""{_dateTimeProviderMock.Object.NowString}"",
                ""currentUserId"": ""test@email.com""
            }}";

            Assert.Equal(expectedJson.Replace(" ", "").Replace("\n", "").Replace("\r", ""), serializedEventGrid.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
        }
    

        [Fact]
        public void JsonSerializer_ShouldDeserializeAsset_WhenAssetType()
        {
            var serializer = JsonSerializerService<InputAsset>.Create(JsonSerializerServiceOptions.AssetOptions);

            string json = @"
            {
                ""id"": 1,
                ""name"": ""TestAsset"",
                ""instances"": 2,
                ""startPos"": [1, 2, 3, 4],
                ""endPos"": [5, 6, 7, 8],
                ""instancePosOffset"": [9, 10, 11, 12],
                ""assetBatchType"": ""ClientRectBatch"",
                ""assetType"": ""Background"",
                ""color"": [13, 14, 15, 16],
                ""text"": ""TestText"",
                ""animationTextureFiles"": [""file1"", ""file2""],
                ""depth"": 17,
                ""scale"": 18
            }";

            var inputAsset = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            Assert.NotNull(inputAsset);
            Assert.Equal(1, inputAsset.Id);
            Assert.Equal("TestAsset", inputAsset.Name);
            Assert.Equal(2, inputAsset.Instances);
            Assert.Equal(new Vector4(1, 2, 3, 4), inputAsset.StartPos);
            Assert.Equal(new Vector4(5, 6, 7, 8), inputAsset.EndPos);
            Assert.Equal(new Vector4(9, 10, 11, 12), inputAsset.InstancePosOffset);
            Assert.Equal(AssetBatchType.ClientRectBatch, inputAsset.AssetBatchType);
            Assert.Equal(AssetType.Background, inputAsset.AssetType);
            Assert.Equal(new Vector4(13, 14, 15, 16), inputAsset.Color);
            Assert.Equal("TestText", inputAsset.Text);
            Assert.Equal(new string[] { "file1", "file2" }, inputAsset.AnimationTextureFiles);
            Assert.Equal(17, inputAsset.Depth);
            Assert.Equal(18, inputAsset.Scale);
        }


        [Fact]
        public void JsonSerializer_ShouldDeserializeEventGrid_WhenEventGridType()
        {
            var serializer = JsonSerializerService<EventItem>.Create(JsonSerializerServiceOptions.EventItemOptions);

            string json = $@"
            {{
                ""id"": ""0"",
                ""resourceId"": 0,
                ""resourceName"": ""Asset"",
                ""currentUserId"": ""test @email.com"",
                ""dateTime"": ""{_dateTimeProviderMock.Object.NowString}""
            }}";

            var eventGridEvent = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));

            Assert.NotNull(eventGridEvent);
            Assert.Equal(0, eventGridEvent.ResourceId);
            Assert.Equal("Asset", eventGridEvent.ResourceName);
            Assert.Equal("test @email.com", eventGridEvent.CurrentUserId);
            Assert.Equal(_dateTimeProviderMock.Object.Now, eventGridEvent.DateTime);
        }
    }
}
