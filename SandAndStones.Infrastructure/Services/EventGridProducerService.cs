using Azure;
using Azure.Messaging.EventGrid;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Contracts;

namespace SandAndStones.Infrastructure.Services
{
    public class EventGridProducerService(
        EventGridSettings producerSettings,
        ILogger<EventGridProducerService> logger
    ) : IProducerService
    {
        private readonly string _topicEndpoint = producerSettings.TopicEndpoint;
        private readonly string _topicKey = producerSettings.TopicKey;
        private readonly ILogger<EventGridProducerService> _logger = logger;

        public async Task PublishMessageAsync(string message)
        {
            var client = new EventGridPublisherClient(new Uri(_topicEndpoint), new AzureKeyCredential(_topicKey));

            var events = new List<EventGridEvent>
            {
                new EventGridEvent(
                    subject: "KafkaProducer",
                    eventType: "KafkaProducer.EventLog",
                    dataVersion: "1.0",
                    data: new { Message = message })
            };

            await client.SendEventsAsync(events);
        }
    }
}
