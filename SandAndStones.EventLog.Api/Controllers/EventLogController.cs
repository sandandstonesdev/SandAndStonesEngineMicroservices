using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;

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
                var message = eventGridEvent.Data.ToString();
                _consumerService.ProcessMessage(message);
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
