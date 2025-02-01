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
            var connectionString = _configuration.GetConnectionString("AppDbConnectionString");
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, "Connection string not found.");
            
            Console.WriteLine(connectionString);
            Console.WriteLine($"Environment: {_enviroment.EnvironmentName}");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    options => {
                        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    });
            });

            var allowedOrigins = _configuration.GetSection("CorsOrigins:AllowedOrigins").Get<string[]>();
            ArgumentNullException.ThrowIfNull(allowedOrigins, nameof(allowedOrigins));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "GatewayApiCorsPolicy",
                                  builder =>
                                  {
                                      builder
                                          .WithOrigins(allowedOrigins)
                                          .AllowAnyHeader()
                                          .AllowAnyMethod()
                                          .AllowCredentials();
                                  });
            });

            services.AddAuthDependencies(_configuration);

            AddMediatRDependencies(services);

            services
                .AddPresentation()
                .AddHttpContextAccessor();
        }

        private void AddMediatRDependencies(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<LoginUserQuery, LoginUserQueryResponse>, LoginUserQueryHandler>();
            services.AddScoped<IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>, RegisterUserCommandHandler>();
            services.AddScoped<IRequestHandler<LogoutUserQuery, LogoutUserQueryResponse>, LogoutUserQueryHandler>();
            services.AddScoped<IRequestHandler<LogoutUserQuery, LogoutUserQueryResponse>, LogoutUserQueryHandler>();
            services.AddScoped<IRequestHandler<CheckCurrentTokenValidityQuery, CheckCurrentTokenValidityQueryResponse>, CheckCurrentTokenValidityQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserInfoQuery, GetUserInfoQueryResponse>, GetUserInfoQueryHandler>();
            services.AddScoped<IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse>, GetUserProfileQueryHandler>();
        }
    }
}
