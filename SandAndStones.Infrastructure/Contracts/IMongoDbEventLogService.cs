using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface IMongoDbEventLogService
    {
        Task LogAsync(EventItem logEntry);
        Task<List<EventItem>> GetAllLogsAsync();
        Task<List<EventItem>> SearchLogsAsync(
            int resourceId,
            string resourceName,
            DateTime? startDateTime,
            DateTime? endDateTime,
            string currentUserId);
    }
}
