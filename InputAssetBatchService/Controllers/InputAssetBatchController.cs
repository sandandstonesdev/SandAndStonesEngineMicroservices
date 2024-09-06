using InputAssetBatchService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InputAssetBatchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputAssetBatchController : ControllerBase
    {
        private readonly IInputAssetBatchRepository _repository;
        public InputAssetBatchController(IInputAssetBatchRepository repository) 
        {
            _repository = repository;
        }

        [Route("/assetBatch/{id}")]
        [HttpGet(Name = "GetInputAssetBatchById")]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            var assetBatch = await _repository.GetById(id);
            if (assetBatch is null)
                return NotFound();

            return Ok(assetBatch);
        }
    }
}
