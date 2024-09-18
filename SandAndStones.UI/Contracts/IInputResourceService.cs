using SandAndStones.UI.DTO;

namespace SandAndStones.UI.Contracts
{
    public interface IInputResourceService<T>
    {
        IEnumerable<string> GetAllNames();
        int GetIdByName(string name);
        T Get(int id);
        IEnumerable<T> GetAll();
    }
}
