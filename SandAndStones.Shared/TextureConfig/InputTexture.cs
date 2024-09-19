namespace SandAndStones.Shared.TextureConfig
{
    public class InputTexture
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public byte[] Data { get; init; }
        public bool Loaded { get; init; }
        public InputTexture()
        {
            Loaded = false;
        }
        public InputTexture(int width, int height, byte[] data) : this()
        {
            this.Width = width;
            this.Height = height;
            if (data is not null && data.Length > 0)
            {
                this.Data = data;
                this.Loaded = true;
            }
        }
    }
}
