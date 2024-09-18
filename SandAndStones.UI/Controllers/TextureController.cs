using InputTextureService.TextureConfig;
using Microsoft.AspNetCore.Mvc;
using SandAndStonesLibrary.AssetConfig;
using SkiaSharp;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace SandAndStones.UI.Controllers
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

        [HttpGet(Name = "GetTexture")]
        public async Task<IActionResult> Get()
        {
            IAsyncTextureReader _inputAssetReader = new InputTextureReader("wall.png");
            InputTexture inputTexture = _inputAssetReader.ReadTextureAsync().Result;
            if (inputTexture.Loaded)
            {
                using var image = SKImage.FromPixelCopy(new SKImageInfo(256, 256), inputTexture.Data);
                using SKBitmap bitmap = SKBitmap.FromImage(image);
                using var data = bitmap.Encode(SKEncodedImageFormat.Png, 0);
                byte[] lastData = MemoryMarshal.AsBytes(data.AsSpan()).ToArray();

                return File(lastData, "image/png", "wall.png");
            }

            return NotFound();
        }
    }
}
