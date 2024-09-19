using Microsoft.AspNetCore.Mvc;
using SandAndStones.Api.DTO;

namespace SandAndStones.Api
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
        [HttpGet()]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            var assetBatch = await _repository.GetById(id);
            if (assetBatch is null)
                return NotFound();

            return Ok(assetBatch);
        }

        [Route("/inputasset/")]
        [HttpGet()]
        public IEnumerable<InputAssetDTO> Get()
        {
            return Enumerable.Range(1, 5).Select(index =>
                                new InputAssetDTO(index,
                                                    $"InputAssetName{index}",
                                                    $"Description: InputAssetName{index} description test")).ToArray();
        }
    }
}
