using Microsoft.Extensions.DependencyInjection;

namespace SandAndStones.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInputAssetBatchRepository, InputAssetBatchRepository>();
            var textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton<IInputTextureRepository>(textureRepository);
        }
    }
}
