namespace SandAndStones.Api.DTO
{
    public record InputTextureDTO
    {
        public int Id { get; set; }
        public int Width { get; init; }

        public int Height { get; init; }

        public byte[] Data { get; init; }

        public InputTextureDTO(int id, int width, int height, byte[] data) 
        {
            this.Id = id;
            this.Width = width;
            this.Height = height;
            this.Data = data;
        }
    }
}
