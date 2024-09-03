using InputAssetBatchService.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandAndStonesEngine.Assets.AssetConfig;

namespace InputAssetBatchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputAssetBatchController : ControllerBase
    {
        IInputAssetBatchRepository _repository;
        public InputAssetBatchController(IInputAssetBatchRepository repository) 
        {
            _repository = repository;
        }

        [Route("/assetBatch/{id}")]
        [HttpGet(Name = "GetInputAssetBatchById")]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            Console.WriteLine($"Getting platform Id: {id}");

            var assetBatch = await _repository.GetById(id);
            if (assetBatch is null)
                return NotFound();

            return Ok(assetBatch);
        }
    }
}
