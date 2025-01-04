using MongoDB.Driver;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Services
{
    public class MongoDbEventLogService : IMongoDbEventLogService
    {
        private readonly IMongoCollection<EventItem> _logCollection;

        public MongoDbEventLogService(string collectionName, IMongoDatabase database)
        {
            _logCollection = database.GetCollection<EventItem>(collectionName);
        }

        public async Task LogAsync(EventItem logEntry)
        {
            await _logCollection.InsertOneAsync(logEntry);
        }

        public async Task<List<EventItem>> GetAllLogsAsync()
        {
            return await SearchLogsAsync(-1, string.Empty, null, null, string.Empty);
        }

        public async Task<List<EventItem>> SearchLogsAsync(
            int resourceId,
            string resourceName,
            DateTime? startDateTime,
            DateTime? endDateTime,
            string currentUserId)
        {
            var filterBuilder = Builders<EventItem>.Filter;
            var filter = filterBuilder.Empty;

            if (resourceId >= 0)
            {
                filter &= filterBuilder.Eq(log => log.ResourceId, resourceId);
            }

            if (!string.IsNullOrEmpty(resourceName))
            {
                filter &= filterBuilder.Eq(log => log.ResourceName, resourceName);
            }

            if (startDateTime.HasValue)
            {
                filter &= filterBuilder.Gte(log => log.DateTime, startDateTime.Value);
            }

            if (endDateTime.HasValue)
            {
                filter &= filterBuilder.Lte(log => log.DateTime, endDateTime.Value);
            }

            if (!string.IsNullOrEmpty(currentUserId))
            {
                filter &= filterBuilder.Eq(log => log.CurrentUserId, currentUserId);
            }

            return await _logCollection.Find(filter).ToListAsync();
        }
    }
}
