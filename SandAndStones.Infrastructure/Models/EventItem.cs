using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SandAndStones.Infrastructure.Services.JsonSerialization;
using System.Text.Json.Serialization;

namespace SandAndStones.Infrastructure.Models
{
    public class EventItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("resourceId")]
        public int ResourceId { get; set; }
        [JsonPropertyName("resourceName")]
        public string ResourceName { get; set; } = string.Empty;
        [JsonPropertyName("dateTime")]
        [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("currentUserId")]
        public string CurrentUserId { get; set; } = string.Empty;
    }
}
