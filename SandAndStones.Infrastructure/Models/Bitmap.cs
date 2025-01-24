namespace SandAndStones.Infrastructure.Models
{
    public class Bitmap
    {
        public string Name { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public byte[] Data { get; init; }
        public bool Loaded { get; init; }
        public string ContentType { get; init; }
        public Bitmap()
        {
            Loaded = false;
        }

        public Bitmap(string name, int width, int height, byte[] data, string contentType) : this()
        {
            Name = name;
            ContentType = "image/png";
            Width = width;
            Height = height;
            ContentType = contentType;
            if (data is not null && data.Length > 0)
            {
                Data = data;
                Loaded = true;
            }
        }

        public Stream GetDataAsStream()
        {
            return new MemoryStream(Data);
        }
    }
}
