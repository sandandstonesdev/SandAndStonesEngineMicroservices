using Microsoft.EntityFrameworkCore;
using SandAndStones.Infrastructure.Data;

namespace SandAndStones.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _enviroment;
        public Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            _configuration = configuration;
            _enviroment = enviroment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInputAssetBatchRepository, InputAssetBatchRepository>();
            var textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton<IInputTextureRepository>(textureRepository);

            var connectionString = _configuration.GetConnectionString("AppDbConnectionString") ?? throw new InvalidOperationException("Connection string not found."); ;
            Console.WriteLine(connectionString);
            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
        }
    }
}
