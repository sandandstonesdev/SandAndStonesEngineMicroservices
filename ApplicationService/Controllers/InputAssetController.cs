using ApplicationService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputAssetController : ControllerBase
    {
        private readonly ILogger<InputAssetController> _logger;

        public InputAssetController(ILogger<InputAssetController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetInputAssetDTO")]
        public IEnumerable<InputAssetDTO> Get()
        {
            return Enumerable.Range(1, 5).Select(index => 
                                new InputAssetDTO(index,
                                                    $"InputAssetName{index}",
                                                    $"Description: InputAssetName{index} description test")).ToArray();
        }
    }
}
