using SkiaSharp;
using System.Runtime.InteropServices;

namespace SandAndStones.Shared.TextureConfig
{
    public class InputTextureReader(string fileName) : IAsyncTextureReader
    {
        private readonly string fileName = fileName;

        public async Task<InputTexture> ReadTextureAsync()
        {
            //string path = GetTextureImageFilePath(fileName);
            var inputTexture = GetImageBytes(fileName);
            return inputTexture;
        }

        public static InputTexture GetImageBytes(string fileName)
        {
            var outputImagePath = GetTextureImageFilePath(fileName);
            using var image = SKImage.FromEncodedData(outputImagePath);
            using var bitmap = SKBitmap.FromImage(image);
            int Width = image.Width;
            int Height = image.Height;
            using var pixmap = bitmap.PeekPixels();
            var span = pixmap.GetPixelSpan();
            byte[] bitmapBytes = MemoryMarshal.AsBytes(span).ToArray();

            return new InputTexture(fileName, Width, Height, bitmapBytes);
        }

        public static string GetTextureImageFilePath(string fileName)
        {
            const string textureImagesPath = "Images";
            string basePath = Path.GetFullPath(@".");
            string imagePath = Path.Combine(basePath, textureImagesPath, fileName);
            return imagePath;
        }
    }
}
