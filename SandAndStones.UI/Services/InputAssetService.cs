using SandAndStones.UI.Contracts;
using SandAndStones.UI.DTO;
using System.Collections;

namespace SandAndStones.UI.Services
{
    public class InputAssetService : IInputAssetService
    {
        public InputAssetService()
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

        public InputAssetDTO Get(int id)
        {
            return new InputAssetDTO(1,"Name", "Desc");
        }

        public IEnumerable<InputAssetDTO> GetAll()
        {
            var inputAssets = new List<InputAssetDTO>();
            {
                new InputAssetDTO(1, "Name", "Desc");
                new InputAssetDTO(1, "Name", "Desc");
            };
            return inputAssets;
        }
    }
}
