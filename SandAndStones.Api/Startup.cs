using Microsoft.EntityFrameworkCore;
using SandAndStones.Infrastructure.Data;

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

            var connectionString = _configuration.GetConnectionString("AppDbConnectionString") ?? throw new InvalidOperationException("Connection string not found."); ;
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
        }
    }
}
