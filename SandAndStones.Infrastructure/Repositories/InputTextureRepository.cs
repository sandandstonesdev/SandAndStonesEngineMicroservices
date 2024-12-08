using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.Shared.TextureConfig;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace SandAndStones.Infrastructure.Repositories
{
    public class InputTextureRepository : IInputTextureRepository
    {
        public Dictionary<int, string> textureDescriptionList = [];
        public InputTextureRepository()
        {
        }

        public void Init()
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
            var inputAssetReader = new InputTextureReader(name);
            var inputTexture = await inputAssetReader.ReadTextureAsync();
            if (inputTexture.Loaded)
            {
                using var image = SKImage.FromPixelCopy(new SKImageInfo(256, 256), inputTexture.Data);
                using SKBitmap bitmap = SKBitmap.FromImage(image);
                using var data = bitmap.Encode(SKEncodedImageFormat.Png, 0);
                byte[] lastData = MemoryMarshal.AsBytes(data.AsSpan()).ToArray();

                return new DownloadTextureDto(name, lastData, "image/png", true);
            }

            return new DownloadTextureDto("EmptyTexture", [], "image/png", false);
        }

        public List<TextureDescription> GetTextureDescriptionList()
        {
            return textureDescriptionList.Select(e => new TextureDescription(e.Key, e.Value)).ToList();
        }
    }
}
