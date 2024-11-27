using SandAndStones.App;

namespace SandAndStones.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInputAssetBatchRepository, InputAssetBatchRepository>();
            var textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton<IInputTextureRepository>(textureRepository);
            
            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "ApiCorsPolicy",
                                  builder =>
                                  {
                                      builder
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader()
                                          .AllowAnyMethod();
                                  });
            });

            services
                .AddPresentation()
                .AddHttpContextAccessor()
                .ConfigureMediatR();
        }
    }
}
