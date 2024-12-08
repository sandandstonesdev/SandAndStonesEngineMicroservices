namespace SandAndStones.Texture.Api.DTO
{
    public record InputTextureDTO
    {
        public int Id { get; set; }
        public int Width { get; init; }

        public int Height { get; init; }

        public byte[] Data { get; init; }

        public InputTextureDTO(int id, int width, int height, byte[] data)
        {
            Id = id;
            Width = width;
            Height = height;
            Data = data;
        }
    }
}
