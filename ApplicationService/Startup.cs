using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService
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
            var connectionString = _configuration.GetConnectionString("AppDbConnectionString") ?? throw new InvalidOperationException("Connection string not found."); ;
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Infrastructure"));
            });
        }
    }
}
