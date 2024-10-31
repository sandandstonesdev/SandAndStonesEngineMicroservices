using SandAndStones.Shared.TextureConfig;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace SandAndStones.Api
{
    [ApiController]
    [Route("[controller]")]
    public class InputTextureController(
        ILogger<InputTextureController> logger,
        IInputTextureRepository inputTextureRepository) : ControllerBase
    {
        private readonly ILogger<InputTextureController> _logger = logger;
        private readonly IInputTextureRepository _inputTextureRepository = inputTextureRepository;

        [Route("/textures")]
        [HttpGet()]
        public async Task<IActionResult> GetTextureDescriptionList()
        {
            var textureDescriptionList = _inputTextureRepository.GetTextureDescriptionList();
            if (textureDescriptionList is null)
                return NotFound();
            return Ok(textureDescriptionList);
        }

        [Route("/textures/{id}")]
        [HttpGet()]
        public async Task<IActionResult> GetTextureById(int id)
        {
            var texture = await _inputTextureRepository.GetById(id);
            if (texture is null)
                return NotFound();
            return Ok(texture);
        }

        [Route("/textureFile/{name}")]
        [HttpGet()]
        public async Task<IActionResult> GetTextureById(string name)
        {
            var _inputAssetReader = new InputTextureReader(name);
            var inputTexture = _inputAssetReader.ReadTextureAsync().Result;
            if (inputTexture.Loaded)
            {
                using var image = SKImage.FromPixelCopy(new SKImageInfo(256, 256), inputTexture.Data);
                using SKBitmap bitmap = SKBitmap.FromImage(image);
                using var data = bitmap.Encode(SKEncodedImageFormat.Png, 0);
                byte[] lastData = MemoryMarshal.AsBytes(data.AsSpan()).ToArray();

                return File(lastData, "image/png", name);
            }

            return NotFound();
        }
    }
}
