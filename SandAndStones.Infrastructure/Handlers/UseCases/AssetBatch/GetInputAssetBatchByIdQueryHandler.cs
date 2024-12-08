using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.AssetBatch.GetInputAssetBatchById;

namespace SandAndStones.Infrastructure.Handlers.UseCases.AssetBatch
{
    public class GetInputAssetBatchByIdQueryHandler(IInputAssetBatchRepository repository) : IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>
    {
        private readonly IInputAssetBatchRepository _repository = repository;
        public async Task<GetInputAssetBatchByIdQueryResponse> Handle(GetInputAssetBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var assetBatch = await _repository.GetById(request.Id);
            if (assetBatch is null)
                return new GetInputAssetBatchByIdQueryResponse(null);
            return new GetInputAssetBatchByIdQueryResponse(assetBatch);
        }
    }
}
