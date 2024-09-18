using SandAndStones.UI.Contracts;
using SandAndStones.UI.DTO;

namespace SandAndStones.UI.Services
{
    public class InputTextureService : IInputTextureService
    {
        public InputTextureService()
        {

        }

        public IEnumerable<string> GetAllNames()
        {
            return new List<string>();
        }

        public int GetIdByName(string name)
        {
            return 0;
        }

        public InputTextureDTO Get(int id)
        {
            return new InputTextureDTO(id, 256, 256, new byte[24]);
        }

        public IEnumerable<InputTextureDTO> GetAll()
        {
            var inputAssets = new List<InputTextureDTO>();
            {
                new InputTextureDTO(0, 256, 256, new byte[24]);
                new InputTextureDTO(1, 256, 256, new byte[24]);
            };
            return inputAssets;
        }
    }
}
