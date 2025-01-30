using SandAndStones.Domain.Entities;
using SandAndStones.Domain.Entities.Texture;

namespace SandAndStones.Domain.Tests
{

    public class TextureTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var name = "test.png";
            var width = 256;
            var height = 256;
            var data = new byte[] { 1, 2, 3 };
            var contentType = "image/png";

            // Act
            var texture = new Texture(name, width, height, data, contentType);

            // Assert
            Assert.Equal(name, texture.Name);
            Assert.Equal(width, texture.Width);
            Assert.Equal(height, texture.Height);
            Assert.Equal(data, texture.Data);
            Assert.True(texture.Loaded);
            Assert.Equal(contentType, texture.ContentType);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            // Arrange
            var width = 256;
            var height = 256;
            var data = new byte[] { 1, 2, 3 };
            var contentType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Texture(null!, width, height, data, contentType));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenWidthIsNegative()
        {
            // Arrange
            var name = "test.png";
            var width = -1;
            var height = 256;
            var data = new byte[] { 1, 2, 3 };
            var contentType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Texture(name, width, height, data, contentType));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenHeightIsNegative()
        {
            // Arrange
            var name = "test.png";
            var width = 256;
            var height = -1;
            var data = new byte[] { 1, 2, 3 };
            var contentType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Texture(name, width, height, data, contentType));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenDataIsNull()
        {
            // Arrange
            var name = "test.png";
            var width = 256;
            var height = -1;
            byte[] data = null!;
            var contentType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Texture(name, width, height, data, contentType));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenDataHasZeroLength()
        {
            // Arrange
            var name = "test.png";
            var width = 256;
            var height = 256;
            byte[] data = [];
            var contentType = "image/png";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Texture(name, width, height, data, contentType));
        }
    }
}