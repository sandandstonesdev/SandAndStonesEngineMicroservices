using System.Numerics;

namespace SandAndStones.Asset.Api
{
    public class AssetInfo
    {
        public Vector3 StartPos { get; init; }
        public Vector3 EndPos { get; init; }
        public AssetInfo()
        {

        }

        private AssetInfo(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            StartPos = new Vector3(startPosX, startPosY, 0);
            EndPos = new Vector3(endPosX, endPosY, 0);
        }

        public static AssetInfo Create2DAssetInfo(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            return new AssetInfo(startPosX, startPosY, endPosX, endPosY);
        }
    }
}
