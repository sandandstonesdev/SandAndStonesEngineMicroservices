using InputTextureService.Repositories;

namespace InputTextureService
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
            IInputTextureRepository textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton(textureRepository);
        }
    }
}
