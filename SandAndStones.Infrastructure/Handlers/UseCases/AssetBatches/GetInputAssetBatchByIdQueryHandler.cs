using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.Contracts.Services;
using SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.Infrastructure.Handlers.UseCases.AssetBatches;

public class GetInputAssetBatchByIdQueryHandler(
    IInputAssetBatchRepository repository,
    IJsonSerializerService<EventItem> jsonSerializerService,
    IProducerService producerService,
    ITokenReaderService tokenReader) : IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>
{
    private readonly IInputAssetBatchRepository _repository = repository;
    private readonly IJsonSerializerService<EventItem> _jsonSerializerService = jsonSerializerService;
    private readonly IProducerService _producerService = producerService;
    private readonly ITokenReaderService _tokenReader = tokenReader;

    public async Task<GetInputAssetBatchByIdQueryResponse> Handle(GetInputAssetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var assetBatch = await _repository.GetById(request.AssetBatchType);
        if (assetBatch is null)
            return new GetInputAssetBatchByIdQueryResponse(null);

        string currentUserEmail = _tokenReader.GetUserEmailFromToken();

        var message = new EventItem { ResourceId = assetBatch.Id, ResourceName = "Asset", CurrentUserId = currentUserEmail, DateTime = DateTime.UtcNow };

        var serializedEvent = _jsonSerializerService.Serialize(message);

        await _producerService.PublishMessageAsync(serializedEvent);

        return new GetInputAssetBatchByIdQueryResponse(assetBatch);
    }
}
