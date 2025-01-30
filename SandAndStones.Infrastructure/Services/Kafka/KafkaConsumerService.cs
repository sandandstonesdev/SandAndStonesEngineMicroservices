using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.Infrastructure.Services.Kafka
{
    public class KafkaConsumerService : BackgroundService, IHostedService, IConsumerService
    {

        private readonly IMongoDbEventLogService _mongoService;

        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IJsonSerializerService<EventItem> _jsonSerializerService;
        private readonly ILogger<KafkaConsumerService> _logger;

        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly string _groupId;
        private readonly string _clientId;

        public KafkaConsumerService
        (
        IMongoDbEventLogService mongoService,
        IJsonSerializerService<EventItem> jsonSerializerService,
        KafkaConsumerSettings config,
        ILogger<KafkaConsumerService> logger
        )
        {
            _mongoService = mongoService;
            _jsonSerializerService = jsonSerializerService;
            _logger = logger;
            _bootstrapServers = config.BootstrapServers;
            _groupId = config.GroupId;
            _topic = config.Topic;
            _clientId = config.ClientId;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = _clientId,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            try
            {

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    ProcessMessage(consumeResult.Message.Value);
                    await Task.Delay(1000, stoppingToken);
                }
            }
            finally
            {
                _consumer.Close();
                _consumer.Dispose();
            }
        }
        public void ProcessMessage(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogInformation($"Received message: {message}");

                    var logEntry = _jsonSerializerService.Deserialize(message);
                    if (logEntry is not null)
                    {
                        _mongoService.LogAsync(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                string message1 = $"Error processing Kafka message: {ex.Message}";
                _logger.LogError(message1);
            }
        }
    }
}
