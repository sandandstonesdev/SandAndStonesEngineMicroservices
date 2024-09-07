using InputTextureService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InputTextureService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputTextureController : ControllerBase
    {
        private readonly ILogger<InputTextureController> _logger;
        private readonly IInputTextureRepository _inputTextureRepository;

        public InputTextureController(ILogger<InputTextureController> logger, IInputTextureRepository inputTextureRepository)
        {
            _logger = logger;
            _inputTextureRepository = inputTextureRepository;
        }

        [Route("/textures")]
        [HttpGet(Name = "GetTextureDescriptionList")]
        public async Task<IActionResult> GetTextureDescriptionList()
        {
            var textureDescriptionList = _inputTextureRepository.GetTextureDescriptionList();
            if (textureDescriptionList is null)
                return NotFound();
            return Ok(textureDescriptionList);
        }

        [Route("/textures/{id}")]
        [HttpGet(Name = "GetTextureById")]
        public async Task<IActionResult> GetTextureById(int id)
        {
            var texture = await _inputTextureRepository.GetById(id);
            if (texture is null)
                return NotFound();
            return Ok(texture);
        }
    }
}
