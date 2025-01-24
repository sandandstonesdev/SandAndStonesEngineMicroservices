﻿using SandAndStones.Domain.Constants;
using SandAndStones.Domain.Entities;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Bitmaps;

namespace SandAndStones.Infrastructure.Services.Blob
{
    public class FileTextureService(IBitmapService bitmapService) : ITextureFileService
    {
        private readonly IBitmapService _bitmapService = bitmapService;
        public async Task<Bitmap> DownloadAsync(string fileName, CancellationToken token = default)
        {
            var bitmap = _bitmapService.Read(fileName);
            if (bitmap is null)
                throw new FileNotFoundException($"Cannot read bitmap: {fileName}");

            return new Bitmap
            (
                bitmap.Name,
                bitmap.Width,
                bitmap.Height,
                _bitmapService.GetRawPixelsFromPng(bitmap.Data),
                bitmap.ContentType
            );
        }

        public async Task<bool> DeleteAsync(string fileName)
        {
            return _bitmapService.Delete(fileName);
        }

        public async Task<Uri> UploadFileAsync(Bitmap bitmap, CancellationToken token = default)
        {
            _bitmapService.Write(bitmap);
            return new Uri(_bitmapService.GetTextureImageFilePath(bitmap.Name));
        }
    }

}
