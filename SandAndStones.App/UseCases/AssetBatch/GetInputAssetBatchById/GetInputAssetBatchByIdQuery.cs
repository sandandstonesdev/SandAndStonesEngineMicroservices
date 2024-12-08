using MediatR;

namespace SandAndStones.App.UseCases.AssetBatch.GetInputAssetBatchById
{
    public record GetInputAssetBatchByIdQuery(int Id) : IRequest<GetInputAssetBatchByIdQueryResponse>;
}
