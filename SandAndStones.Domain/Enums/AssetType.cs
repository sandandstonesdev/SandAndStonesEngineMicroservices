namespace SandAndStones.Domain.Enums
{
    public enum AssetType : uint
    {
        Unknown = 0,
        Background = 1,
        Sprite = 2,
        CharacterSprite = 4,
        Text = 8,
        FPSText = 16,
        PointsText = 32
    }
}
