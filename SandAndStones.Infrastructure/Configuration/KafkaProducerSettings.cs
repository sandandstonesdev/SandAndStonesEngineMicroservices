namespace SandAndStones.Infrastructure.Configuration
{
    public class KafkaProducerSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string BootstrapServers { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
    }
}
