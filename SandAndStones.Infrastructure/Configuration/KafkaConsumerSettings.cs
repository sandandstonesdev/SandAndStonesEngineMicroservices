namespace SandAndStones.Infrastructure.Configuration
{
    public class KafkaConsumerSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string BootstrapServers { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
    }
}
