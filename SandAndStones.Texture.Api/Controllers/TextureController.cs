using MediatR;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.GetTextureById;
using SandAndStones.App.UseCases.Texture.GetTexturesDecriptions;

namespace SandAndStones.Texture.Api.Controllers
{
    [ApiController]
    [Route("texture-api")]
    public class TextureController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("textures")]
        public async Task<IActionResult> GetTexturesDescription()
        {
            var result = await _mediator.Send(new GetTexturesDescriptionsQuery());
            if (result.TextureDescriptions.Count == 0)
                return NotFound();
            return Ok(new { result.TextureDescriptions });
        }

        [HttpGet("textures/{id}")]
        public async Task<IActionResult> GetTextureById(int id)
        {
            var result = await _mediator.Send(new GetTextureByIdQuery(id));
            if (result is null)
                return NotFound();
            return Ok(new { result.Texture });
        }

        [HttpGet("textureFile/{name}")]
        public async Task<IActionResult> DownloadTextureByName(string name)
        {
            var result = await _mediator.Send(new DownloadTextureByNameQuery(name));
            if (result is null || !result.Loaded)
                return NotFound();
            return File(result.FileData, result.ContentType, result.FileName);
        }
    }
}
