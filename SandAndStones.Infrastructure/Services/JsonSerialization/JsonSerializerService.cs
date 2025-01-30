using Azure.Messaging.EventGrid.SystemEvents;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Models.Assets;
using System.Text.Json;

namespace SandAndStones.Infrastructure.Services.JsonSerialization
{
    public class JsonSerializerService<T> : IJsonSerializerService<T> where T : class
    {
        private readonly JsonSerializerOptions _options;
        private JsonSerializerService(JsonSerializerOptions options)
        {
            _options = options;
        }

        public static IJsonSerializerService<T> Create(JsonSerializerOptions options)
        {
            var templateType = typeof(T);
            if (templateType != typeof(InputAsset)
                && templateType != typeof(InputAssetBatch)
                && templateType != typeof(EventItem)
                && templateType != typeof(SubscriptionValidationEventData))
            {
                throw new Exception("Invalid type");
            }
            return new JsonSerializerService<T>(options);
        }

        public string Serialize(T data)
        {
            return JsonSerializer.Serialize(data, _options);
        }

        public T Deserialize(Stream serializedStream)
        {
            var data = JsonSerializer.Deserialize<T>(serializedStream, _options);
            return data is null ? throw new Exception("Deserialization failed") : data;
        }

        public T Deserialize(string serializedString)
        {
            var data = JsonSerializer.Deserialize<T>(serializedString, _options);
            return data is null ? throw new Exception("Deserialization failed") : data;
        }
    }
}
