
namespace SandAndStones.Infrastructure.Services.JsonSerialization
{
    public interface IJsonSerializerService<T> where T : class
    {
        T Deserialize(Stream serializedStream);
        string Serialize(T data);
        public T Deserialize(string serializedString);
    }
}