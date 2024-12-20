namespace SandAndStones.Shared.TextureConfig
{
    public class InputTexture
    {
        public string Name { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public byte[] Data { get; init; }
        public bool Loaded { get; init; }
        public string ContentType { get; init; }
        public InputTexture()
        {
            Loaded = false;
        }
        public InputTexture(string name, string contentType, Stream stream) : this()
        {
            this.Name = name;
            this.ContentType = contentType;
            this.Width = 256;
            this.Height = 256;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                this.Data = ms.ToArray();
                this.Loaded = true;
            }
        }

        public InputTexture(string name, int width, int height, byte[] data) : this()
        {
            this.Name = name;
            this.ContentType = "image/png";
            this.Width = width;
            this.Height = height;
            if (data is not null && data.Length > 0)
            {
                this.Data = data;
                this.Loaded = true;
            }
        }

        public Stream GetDataAsStream()
        {
            return new MemoryStream(Data);
        }
    }
}
