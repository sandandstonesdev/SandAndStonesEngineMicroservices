using MediatR;
using Microsoft.EntityFrameworkCore;
using SandAndStones.App.UseCases.Profile.GetUserProfile;
using SandAndStones.App.UseCases.User.CheckCurrentTokenValidity;
using SandAndStones.App.UseCases.User.GetUserInfo;
using SandAndStones.App.UseCases.User.LoginUser;
using SandAndStones.App.UseCases.User.LogoutUser;
using SandAndStones.App.UseCases.User.RegisterUser;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Handlers.UseCases.Profile;
using SandAndStones.Infrastructure.Handlers.UseCases.User;

namespace SandAndStones.Gateway.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IWebHostEnvironment _enviroment = enviroment;

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

            Console.WriteLine(connectionString);
            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    options => {
                        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(7), null);
                    });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GatewayApiCorsPolicy",
                                  builder =>
                                  {
                                      var corsOrigin = _configuration["JwtSettings:Issuer"] ?? "https://localhost:5173";
                                      builder
                                          .WithOrigins(corsOrigin,
                                                       "https://localhost:5000",
                                                       "https://sand-and-stones-client-app-0001-cyg9asb6eahgf6ab.canadacentral-01.azurewebsites.net")
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .AllowCredentials();
                                  });
            });

            services.AddAuthDependencies(_configuration);

            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<LoginUserQuery, LoginUserQueryResponse>, LoginUserQueryHandler>();
            services.AddScoped<IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>, RegisterUserCommandHandler>();
            services.AddScoped<IRequestHandler<LogoutUserQuery, LogoutUserQueryResponse>, LogoutUserQueryHandler>();
            services.AddScoped<IRequestHandler<LogoutUserQuery, LogoutUserQueryResponse>, LogoutUserQueryHandler>();
            services.AddScoped<IRequestHandler<CheckCurrentTokenValidityQuery, CheckCurrentTokenValidityQueryResponse>, CheckCurrentTokenValidityQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserInfoQuery, GetUserInfoQueryResponse>, GetUserInfoQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse>, GetUserProfileQueryHandler>();

            services
                .AddPresentation()
                .AddHttpContextAccessor();
        }
    }
}
