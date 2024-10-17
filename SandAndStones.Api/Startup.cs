using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var server = _configuration["DbServer"] ?? "";
            var port = _configuration["DbPort"] ?? "";
            var user = _configuration["DbUser"] ?? "";
            var password = _configuration["DbPass"] ?? "";
            var dbName = _configuration["DbName"] ?? "";
            
            var connectionString = _enviroment.IsDevelopment() ?
                _configuration.GetConnectionString("AppDbConnectionString") :
                $"Server={server}, {port}; Database={dbName}; User Id={user}; Password={password};TrustServerCertificate=true";

            _ = connectionString ?? throw new InvalidOperationException("Connection string not found."); ;

            services.AddTransient<IInputAssetBatchRepository, InputAssetBatchRepository>();
            var textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton<IInputTextureRepository>(textureRepository);

            Console.WriteLine(connectionString);
            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddCors(options =>
            {
            
                options.AddPolicy(name: "ApiCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost",
                                                          "https://sand-and-stones-client-app-0001-cyg9asb6eahgf6ab.canadacentral-01.azurewebsites.net")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });
        }
    }
}
