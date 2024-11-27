using Microsoft.AspNetCore.Mvc;

namespace SandAndStones.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputAssetBatchController(IInputAssetBatchRepository repository) : ControllerBase
    {
        private readonly IInputAssetBatchRepository _repository = repository;
        
        [Route("/assetBatch/{id}")]
        [HttpGet()]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            var assetBatch = await _repository.GetById(id);
            if (assetBatch is null)
                return NotFound();

            return Ok(assetBatch);
        }
    }
}
