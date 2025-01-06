using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

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

                    string cleanedMessage = message.Replace("\\n", "").Replace("\\r", "");
                    string unescapedMessage = Regex.Unescape(cleanedMessage);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var logEntry = JsonSerializer.Deserialize<EventItem>(unescapedMessage, options);
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
