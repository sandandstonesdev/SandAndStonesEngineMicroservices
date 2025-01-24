using MediatR;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Texture.Api.DTO;

namespace SandAndStones.Texture.Api.Controllers
{
    [ApiController]
    [Route("texture-api")]
    public class TextureController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("textureFile/{name}")]
        public async Task<IActionResult> DownloadTextureByName(string name)
        {
            var result = await _mediator.Send(new DownloadTextureByNameQuery(name));
            if (result is null || !result.Loaded)
                return NotFound();
            return File(result.FileData, result.ContentType, result.FileName);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTexture([FromBody]UploadTextureFileDTO file)
        {
            var fileData = Convert.FromBase64String(file.Base64Data);


            var result = await _mediator.Send(new UploadTextureCommand(
                file.Name,
                256,
                256,
                fileData
            ));

            if (!result.Uploaded)
                return BadRequest();

            return Ok();
        }
    }
}
