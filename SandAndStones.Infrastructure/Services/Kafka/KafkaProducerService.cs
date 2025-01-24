using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Services.Kafka
{
    public class KafkaProducerService : IProducerService
    {
        private string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        private readonly string _bootstrapServers;
        private readonly string _clientId;

        public KafkaProducerService(KafkaProducerSettings config, ILogger<KafkaProducerService> logger)
        {
            _logger = logger;
            _bootstrapServers = config.BootstrapServers; ;
            _topic = config.Topic;
            _clientId = config.ClientId;

            var producerconfig = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = _clientId,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
        }

        public async Task PublishMessageAsync(string message)
        {
            var kafkaMessage = new Message<Null, string>
            {
                Value = message
            };

            try
            {
                var result = await _producer.ProduceAsync(_topic, kafkaMessage);
                _logger.LogInformation($"Message sent to {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
            }
        }

    }
}
