using MediatR;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.App.UseCases.AssetBatch.GetInputAssetBatchById;

namespace SandAndStones.Asset.Api.Controllers
{
    [ApiController]
    [Route("asset-api")]
    public class AssetBatchController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("assetBatch/{id}")]
        public async Task<IActionResult> GetInputAssetBatchById(int id)
        {
            var result = await _mediator.Send(new GetInputAssetBatchByIdQuery(id));
            if (result.InputAssetBatch is null)
                return NotFound();
            return Ok(result.InputAssetBatch);
        }
    }
}
