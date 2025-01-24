using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Models;
using System.IO.Abstractions;

namespace SandAndStones.Infrastructure.Services.Bitmaps
{
    public class BitmapService : IBitmapService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IAppContextWrapper _appContextWrapper;
        private readonly ISkiaWrapper _skImageWrapper;

        public BitmapService(
            IAppContextWrapper appContextWrapper,
            IFileSystem fileSystem,
            ISkiaWrapper skImageWrapper)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _appContextWrapper = appContextWrapper ?? throw new ArgumentNullException(nameof(appContextWrapper));
            _skImageWrapper = skImageWrapper ?? throw new ArgumentNullException(nameof(skImageWrapper));
        }

        public IEnumerable<string> EnumerateBitmaps(string imageDir = "Images")
        {
            
            string basePath = _fileSystem.Directory.GetCurrentDirectory();
            string imagePath = _fileSystem.Path.Combine(basePath, imageDir);
            return _fileSystem.Directory.EnumerateFiles(imagePath).Select(x => Path.GetFileName(x));
        }
        public Bitmap Read(string fileName)
        {
            var imagePath = GetTextureImageFilePath(fileName);

            byte[] pngData = _fileSystem.File.ReadAllBytes(imagePath);
            
            var (bitmapBytes, width, height) = _skImageWrapper.GetRawPixelsFromPng(pngData);

            return new Bitmap(fileName, width, height, bitmapBytes, MediaType.ImagePng);
        }

        public void Write(Bitmap bitmap)
        {
            var outputImagePath = GetTextureImageFilePath(bitmap.Name);

            var (data, _, _) = _skImageWrapper.GetRawPixelsFromPng(bitmap.Data);
            using var stream = _fileSystem.File.OpenWrite(outputImagePath);
            stream.Write(data, 0, data.Length);
        }

        public bool Delete(string filename)
        {
            var imagePath = GetTextureImageFilePath(filename);
            if (!_fileSystem.File.Exists(imagePath))
                return false;

            _fileSystem.File.Delete(imagePath);
            return true;
        }

        public byte[] GetRawPixelsFromPng(byte[] rawData)
        {
            var (rawPixels, _, _) = _skImageWrapper.GetRawPixelsFromPng(rawData);
            return rawPixels;
        }

        public byte[] GetPngPixelsFromRaw(byte[] rawData, int width = 256, int height = 256)
        {
            return _skImageWrapper.GetPngPixelsFromRaw(rawData, width, height);
        }

        public bool ValidatePngSignature(byte[] data)
        {
            if (data[0] != 0x89 || data[1] != (byte)'P' || data[2] != (byte)'N' || data[3] != (byte)'G')
                return false;
            return true;
        }

        public (int, int) ReadPngMetadata(byte[] data)
        {
            int width = BitConverter.ToInt32([data[19], data[18], data[17], data[16]], 0);
            int height = BitConverter.ToInt32([data[23], data[22], data[21], data[20]], 0);
            return (width, height);
        }

        public string GetTextureImageFilePath(string fileName, string imageDir = "Images")
        {
            string basePath = _appContextWrapper.BaseDirectory;
            string imagePath = _fileSystem.Path.Combine(basePath, imageDir, fileName);
            return imagePath;
        }
    }
}
