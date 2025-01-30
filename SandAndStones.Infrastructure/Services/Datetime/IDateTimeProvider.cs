namespace SandAndStones.Infrastructure.Services.Datetime
{
    public interface IDateTimeProvider
    {
        DateTimeOffset NowOffset { get; }
        DateTime Now { get; }
        string NowString { get; }
        string NowOffsetString { get; }
    }
}