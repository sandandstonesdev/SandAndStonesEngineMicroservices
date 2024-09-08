using ApplicationService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextureController : ControllerBase
    {
        private readonly ILogger<TextureController> _logger;

        public TextureController(ILogger<TextureController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetInputAssetDTOForTexture")]
        public IEnumerable<InputAssetDTO> Get()
        {
            return Enumerable.Range(1, 5).Select(index =>
                                new InputAssetDTO(index,
                                                    $"InputAssetName{index}",
                                                    $"Description: InputAssetName{index} description test")).ToArray();
        }
    }
}
