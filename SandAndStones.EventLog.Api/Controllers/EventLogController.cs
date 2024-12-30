using Microsoft.AspNetCore.Mvc;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.EventLog.Api.Controllers
{
    [ApiController]
    [Route("eventlog-api")]
    public class EventLogController(
        IMongoDbEventLogService mongoService) : ControllerBase
    {
        private readonly IMongoDbEventLogService _mongoService = mongoService;

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
