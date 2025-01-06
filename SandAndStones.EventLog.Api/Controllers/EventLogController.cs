using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Text.Json;

namespace SandAndStones.EventLog.Api.Controllers
{
    [ApiController]
    [Route("eventlog-api")]
    public class EventLogController(
        IMongoDbEventLogService mongoService,
        IConsumerService consumerService) : ControllerBase
    {
        private readonly IMongoDbEventLogService _mongoService = mongoService;
        private readonly IConsumerService _consumerService = consumerService;

        [HttpPost("events")]
        public async Task<IActionResult> ReceiveEvents([FromBody] EventGridEvent[] events)
        {
            foreach (var eventGridEvent in events)
            {
                if (eventGridEvent.EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    var validationEventData = JsonSerializer.Deserialize<SubscriptionValidationEventData>(eventGridEvent.Data.ToString());
                    var responseData = new SubscriptionValidationResponse
                    {
                        ValidationResponse = validationEventData?.ValidationCode
                    };
                    return Ok(responseData);
                }
                else
                {
                    var message = eventGridEvent.Data.ToString();
                    _consumerService.ProcessMessage(message);
                    return Ok(message);
                }
            }

            return Ok();
        }

        [HttpGet("eventList")]
        public async Task<IActionResult> GetInputAssetBatchEvents()
        {
            List<EventItem> eventList = await _mongoService.GetAllLogsAsync();
            if (eventList is null)
                return NotFound();

            return Ok(eventList);
        }
    }
}
