using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Services.Bitmaps
{
    public interface IBitmapService
    {
        IEnumerable<string> EnumerateBitmaps(string path = "./Images");
        Bitmap Read(string filename);
        void Write(Bitmap bitmap);
        bool Delete(string filename);
        bool ValidatePngSignature(byte[] data);
        (int, int) ReadPngMetadata(byte[] data);
        byte[] GetRawPixelsFromPng(byte[] rawData);
        byte[] GetPngPixelsFromRaw(byte[] rawData, int width = 256, int height = 256);
        string GetTextureImageFilePath(string fileName, string imageDir = "Images");
    }
}
