using SandAndStones.App.Contracts.Repository;
using SandAndStones.Domain.Entities.Assets;
using SandAndStones.Domain.Enums;
using SandAndStones.Infrastructure.Models.Assets;
using SandAndStones.Infrastructure.Services.Assets;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.Infrastructure.Repositories
{
    public class InputAssetBatchRepository
    (
        IAsyncAssetReader asyncAssetReader
    ) : IInputAssetBatchRepository
    {
        private readonly IAsyncAssetReader _asyncAssetReader = asyncAssetReader;

        public async Task<AssetBatch> GetById(AssetBatchType assetType)
        {
            var batchData = await _asyncAssetReader.ReadBatchAsync(assetType);

            var assetBatch = new AssetBatch(
                batchData.Id,
                batchData.InputAssets.Select(asset =>
                new Asset(
                    asset.Id,
                    asset.Name,
                    asset.Instances,
                    asset.StartPos,
                    asset.EndPos,
                    asset.InstancePosOffset,
                    asset.AssetBatchType,
                    asset.AssetType,
                    asset.Color,
                    asset.Text,
                    asset.AnimationTextureFiles,
                    asset.Depth,
                    asset.Scale
                 )).ToList()
                );

            return assetBatch;
        }
    }
}
