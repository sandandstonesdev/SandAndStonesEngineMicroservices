using SandAndStones.Infrastructure.Models;
using SkiaSharp;

namespace SandAndStones.Infrastructure.Services.Bitmaps
{
    public interface ISkiaWrapper
    {
        (byte[], int, int) FromPngFile(string path);
        byte[] GetPngPixelsFromRaw(byte[] pixels, int width, int height);
        (byte[], int, int) GetRawPixelsFromPng(byte[] pixels);
    }
}
