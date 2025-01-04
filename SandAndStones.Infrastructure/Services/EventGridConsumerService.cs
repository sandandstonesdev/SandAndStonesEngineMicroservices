using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Net;
using System.Text.Json;

namespace SandAndStones.Infrastructure.Services
{
    public class EventGridConsumerService(
        IMongoDbEventLogService mongoService,
        ILogger<EventGridConsumerService> logger) : IConsumerService
    {
        private readonly IMongoDbEventLogService _mongoService = mongoService;
        private readonly ILogger<EventGridConsumerService> _logger = logger;

        public void ProcessMessage(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogInformation($"Received message: {message}");

                    var logEntry = JsonSerializer.Deserialize<EventItem>(message);
                    if (logEntry != null)
                    {
                        _mongoService.LogAsync(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Event Grid message: {ex.Message}");
            }
        }
    }
}
