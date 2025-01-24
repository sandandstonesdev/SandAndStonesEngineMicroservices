namespace SandAndStones.Infrastructure.Services.Bitmaps
{

    public class AppContextWrapper : IAppContextWrapper
    {
        public string BaseDirectory => AppContext.BaseDirectory;
    }
}
