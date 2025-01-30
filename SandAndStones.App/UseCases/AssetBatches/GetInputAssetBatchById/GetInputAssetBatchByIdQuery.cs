using MediatR;
using SandAndStones.Domain.Enums;

namespace SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById
{
    public record GetInputAssetBatchByIdQuery(AssetBatchType AssetBatchType) : IRequest<GetInputAssetBatchByIdQueryResponse>;
}
