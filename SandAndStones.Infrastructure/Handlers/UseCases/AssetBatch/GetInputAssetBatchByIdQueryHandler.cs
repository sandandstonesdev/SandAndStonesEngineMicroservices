using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.Contracts.Services;
using SandAndStones.App.UseCases.AssetBatch.GetInputAssetBatchById;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using SandAndStones.Infrastructure.Services.Asset;

namespace SandAndStones.Infrastructure.Handlers.UseCases.AssetBatch;

public class GetInputAssetBatchByIdQueryHandler(
    IInputAssetBatchRepository repository,
    IProducerService producerService,
    ITokenReaderService tokenReader) : IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>
{
    private readonly IInputAssetBatchRepository _repository = repository;
    private readonly IProducerService _producerService = producerService;
    private readonly ITokenReaderService _tokenReader = tokenReader;

    public async Task<GetInputAssetBatchByIdQueryResponse> Handle(GetInputAssetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var assetBatch = await _repository.GetById(request.Id);
        if (assetBatch is null)
            return new GetInputAssetBatchByIdQueryResponse(null);

        string currentUserEmail = _tokenReader.GetUserEmailFromToken();

        var message = new EventItem { ResourceId = assetBatch.Id, ResourceName = "Asset", CurrentUserId = currentUserEmail, DateTime = DateTime.UtcNow };

        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await _producerService.PublishMessageAsync(JsonSerializer.Serialize(message, jsonSerializerOptions));
        return new GetInputAssetBatchByIdQueryResponse(assetBatch);
    }
}
