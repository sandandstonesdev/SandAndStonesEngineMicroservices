using SandAndStones.Infrastructure.Models;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace SandAndStones.Infrastructure.Services.Bitmaps
{
    public class SkiaWrapper : ISkiaWrapper
    {
        public byte[] GetPngPixelsFromRaw(byte[] pixels, int width, int height)
        {
            using var image = SKImage.FromPixelCopy(new SKImageInfo(width, height), pixels);
            SKBitmap bitmap = SKBitmap.FromImage(image);
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 0);
            byte[] lastData = MemoryMarshal.AsBytes(data.AsSpan()).ToArray();
            return lastData;
        }

        public (byte[], int, int) GetRawPixelsFromPng(byte[] pixels)
        {
            using var image = SKImage.FromEncodedData(pixels);
            SKBitmap bitmap = SKBitmap.FromImage(image);
            int width = image.Width;
            int height = image.Height;
            using var pixmap = bitmap.PeekPixels();
            var span = pixmap.GetPixelSpan();
            byte[] bitmapBytes = MemoryMarshal.AsBytes(span).ToArray();
            return (bitmapBytes, width, height);
        }

        public (byte[], int, int) FromPngFile(string path)
        {
            using var data = SKData.Create(path);
            using var image = SKImage.FromEncodedData(data);
            using var bitmap = SKBitmap.FromImage(image);
            int width = image.Width;
            int height = image.Height;
            using var pixmap = bitmap.PeekPixels();
            var span = pixmap.GetPixelSpan();
            byte[] bitmapBytes = MemoryMarshal.AsBytes(span).ToArray();
            return (bitmapBytes, width, height);
        }
    }
}
