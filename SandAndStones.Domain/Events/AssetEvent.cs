namespace SandAndStones.Domain.Events
{
    public class AssetEvent
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string CurrentUserId { get; set; } = string.Empty;

        public AssetEvent(int id, int resourceId, string resourceName, string currentUserId, DateTime dateTime)
        {
            Id = id;
            ResourceId = resourceId;
            ResourceName = resourceName;
            CurrentUserId = currentUserId;
            DateTime = dateTime;
        }
    }
}
