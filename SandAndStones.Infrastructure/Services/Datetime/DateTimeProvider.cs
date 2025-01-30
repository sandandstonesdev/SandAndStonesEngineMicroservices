namespace SandAndStones.Infrastructure.Services.Datetime
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";

        public DateTime Now => DateTime.UtcNow;
        public DateTimeOffset NowOffset => new DateTimeOffset(Now);

        public string NowString => Now.ToString(DateTimeFormat);

        public string NowOffsetString => NowOffset.ToString(DateTimeOffsetFormat);
    }
}
