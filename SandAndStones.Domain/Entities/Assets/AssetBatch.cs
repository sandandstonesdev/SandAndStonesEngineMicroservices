namespace SandAndStones.Domain.Entities.Assets
{
    public class AssetBatch
    {
        public int Id { get; set; }
        public List<Asset> Assets { get; set; }

        public AssetBatch(int id, List<Asset> inputAssets)
        {
            Id = id;
            Assets = inputAssets;
        }
    }
}
