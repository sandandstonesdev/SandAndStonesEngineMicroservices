using Microsoft.AspNetCore.Mvc;

namespace SandAndStones.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InputAssetBatchController(IInputAssetBatchRepository repository) : ControllerBase
    {
        private readonly IInputAssetBatchRepository _repository = repository;
        
        [HttpGet("assetBatch/{id}")]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            var assetBatch = await _repository.GetById(id);
            if (assetBatch is null)
                return NotFound();

            return Ok(assetBatch);
        }
    }
}
