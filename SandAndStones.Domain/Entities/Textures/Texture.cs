using SandAndStones.Domain.Constants;

namespace SandAndStones.Domain.Entities.Texture
{
    public class Texture
    {
        public string Name { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public byte[] Data { get; init; }
        public bool Loaded { get; init; }
        public string ContentType { get; init; } = MediaType.ImagePng;

        public Texture(string name, int width, int height, byte[] data, string contentType = MediaType.ImagePng)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width, nameof(width));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height, nameof(height));
            ArgumentNullException.ThrowIfNull(data, nameof(data));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(data.Length, nameof(data.Length));

            Name = name;
            Width = width;
            Height = height;
            Data = data;
            Loaded = true;
        }
    }
}
