using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.EventLog.Api.Controllers
{
    [ApiController]
    [Route("eventlog-api")]
    public class EventLogController(
        IMongoDbEventLogService mongoService,
        IConsumerService consumerService,
        IJsonSerializerService<SubscriptionValidationEventData> jsonSerializerService) : ControllerBase
    {
        private readonly IMongoDbEventLogService _mongoService = mongoService;
        private readonly IConsumerService _consumerService = consumerService;
        private readonly IJsonSerializerService<SubscriptionValidationEventData> _jsonSerializerService = jsonSerializerService;

        [HttpPost("events")]
        public async Task<IActionResult> ReceiveEvents([FromBody] EventGridEvent[] events)
        {
            foreach (var eventGridEvent in events)
            {
                if (eventGridEvent.EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    var validationEventData = _jsonSerializerService.Deserialize(eventGridEvent.Data.ToStream());
                    var responseData = new SubscriptionValidationResponse
                    {
                        ValidationResponse = validationEventData?.ValidationCode
                    };
                    return await Task.FromResult(Ok(responseData));
                }
                else
                {
                    var message = eventGridEvent.Data.ToString();
                    _consumerService.ProcessMessage(message);
                    return await Task.FromResult(Ok(message));
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
