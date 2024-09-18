using InputTextureService.TextureConfig;

namespace SandAndStones.Api
{
    public class InputTextureRepository : IInputTextureRepository
    {
        public Dictionary<int, string> textureDescriptionList = new Dictionary<int, string>();
        public InputTextureRepository() 
        {
        }

        public void Init()
        {
            int id = 0;
            const string path = "./Images";
            foreach(var filePath in Directory.EnumerateFiles(path))
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

        public List<TextureDescription> GetTextureDescriptionList()
        {
            return textureDescriptionList.Select(e => new TextureDescription(e.Key, e.Value)).ToList();
        }
    }
}
