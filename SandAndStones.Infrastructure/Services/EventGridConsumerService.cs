using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
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

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = null // Use null to keep property names as-is
                    };

                    var logEntry = JsonSerializer.Deserialize<EventItem>(message, options);
                    if (logEntry != null)
                    {
                        _mongoService.LogAsync(logEntry);
                    }
                    else
                    {
                        _logger.LogWarning("Deserialized log entry is null.");
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, $"JSON deserialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Event Grid message: {ex.Message}");
            }
        }
    }
}
