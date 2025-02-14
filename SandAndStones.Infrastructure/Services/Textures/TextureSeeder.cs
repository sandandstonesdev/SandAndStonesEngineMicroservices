﻿using Microsoft.Extensions.Hosting;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Services.Bitmaps;

namespace SandAndStones.Infrastructure.Services.Textures
{
    public class TextureSeeder(IBitmapService bitmapService, ITextureRepository textureRepository) : IHostedService
    {
        private readonly IBitmapService _bitmapService = bitmapService;
        private readonly ITextureRepository textureRepository = textureRepository;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool success = await SeedTextures();
            if (!success)
            {
                throw new FileLoadException("Failed to seed textures.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> SeedTextures()
        {
            try
            {
                var existingTextures = await textureRepository.ListTexturesAsync();

                foreach (var textureName in _bitmapService.EnumerateBitmaps())
                {
                    if (existingTextures.Contains(textureName))
                        continue;

                    var bitmap = _bitmapService.Read(textureName);
                    var result = await textureRepository.UploadTexture(bitmap.Name, bitmap.Width, bitmap.Height, bitmap.Data, MediaType.ImagePng);
                    if (!result.Loaded)
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while seeding textures: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}
