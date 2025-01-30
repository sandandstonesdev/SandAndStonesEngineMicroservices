using MediatR;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById;
using SandAndStones.Domain.Enums;

namespace SandAndStones.Asset.Api.Controllers
{
    [ApiController]
    [Route("asset-api")]
    public class AssetBatchController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("assetBatch/{assetBatchType}")]
        public async Task<IActionResult> GetInputAssetBatchById(AssetBatchType assetBatchType)
        {
            var result = await _mediator.Send(new GetInputAssetBatchByIdQuery(assetBatchType));
            if (result.InputAssetBatch is null)
                return NotFound();
            return Ok(result.InputAssetBatch);
        }
    }
}
