using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SandAndStones.Infrastructure.Models
{
    public class EventItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string CurrentUserId { get; set; } = string.Empty;
    }
}
