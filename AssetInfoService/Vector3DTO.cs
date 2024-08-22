using System.Numerics;

namespace AssetInfoService
{
    public class Vector3DTO
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3DTO()
        {

        }

        public static List<Vector3DTO> FromAssetInfoPos(AssetInfo assetInfo)
        {
            return new() 
            {
                new Vector3DTO()
                {
                    X = assetInfo.StartPos.X,
                    Y = assetInfo.StartPos.Y,
                    Z = assetInfo.StartPos.Z
                },
                new Vector3DTO()
                {
                    X = assetInfo.StartPos.X,
                    Y = assetInfo.StartPos.Y,
                    Z = assetInfo.StartPos.Z
                }
            };
        }
    }
}
