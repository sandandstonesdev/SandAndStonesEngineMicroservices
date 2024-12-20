using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Shared.TextureConfig;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace SandAndStones.Infrastructure.Repositories
{
    public class InputTextureRepository : IInputTextureRepository
    {
        public Dictionary<int, string> textureDescriptionList = [];
        private readonly IAzureBlobService _azureBlobService;

        public InputTextureRepository(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
            Init();
        }
        private void Init()
        {
            int id = 0;
            const string path = "./Images";
            foreach (var filePath in Directory.EnumerateFiles(path))
            {
                var fileName = Path.GetFileName(filePath);
                if (!textureDescriptionList.ContainsValue(fileName))
                {
                    textureDescriptionList.Add(id++, fileName);
                }
            }
        }

        public async Task<bool> SeedTextures()
        {
            foreach (var texture in textureDescriptionList)
            {
                var reader = new InputTextureReader(texture.Value);
                var textureData = await reader.ReadTextureAsync();
                var result = await UploadTexture(texture.Value, textureData.Width, textureData.Height, textureData.Data);
                if (!result.Loaded)
                {
                    return false;
                }
            }

            return true;
        }

        private string GetTextureNameById(int id)
        {
            return textureDescriptionList[id];
        }

        public async Task<InputTexture> GetById(int id)
        {
            var textureName = GetTextureNameById(id);
            var reader = new InputTextureReader(textureName);
            var textureData = await reader.ReadTextureAsync();

            return textureData;
        }

        public async Task<DownloadTextureDto> DownloadTextureByName(string name)
        {
            var inputTexture = await _azureBlobService.DownloadAsync(name);

            if (inputTexture.Loaded)
            {
                return new DownloadTextureDto(name, GetBytesAsPng(inputTexture.Data), "image/png", true);
            }

            return new DownloadTextureDto("EmptyTexture", [], "image/png", false);
        }

        private byte[] GetBytesAsPng(byte[] rawData)
        {
            using var image = SKImage.FromPixelCopy(new SKImageInfo(256, 256), rawData);
            using SKBitmap bitmap = SKBitmap.FromImage(image);
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 0);
            byte[] lastData = MemoryMarshal.AsBytes(data.AsSpan()).ToArray();
            return lastData;
        }

        public List<TextureDescription> GetTextureDescriptionList()
        {
            return textureDescriptionList.Select(e => new TextureDescription(e.Key, e.Value)).ToList();
        }

        public async Task<UploadTextureDto> UploadTexture(string name, int width, int height, byte[] data)
        {
            var inputTexture = new InputTexture(name, width, height, data);
            var uri = await _azureBlobService.UploadFileAsync(inputTexture.Name, inputTexture.GetDataAsStream());
            
            return new UploadTextureDto(
                inputTexture.Name,
                inputTexture.Data,
                inputTexture.ContentType,
                inputTexture.Loaded);
        }
    }
}
