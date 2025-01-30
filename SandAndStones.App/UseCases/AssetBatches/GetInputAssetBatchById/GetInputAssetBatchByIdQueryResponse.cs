using SandAndStones.Domain.Entities.Assets;

namespace SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById
{
    public record GetInputAssetBatchByIdQueryResponse(AssetBatch? InputAssetBatch);
}
