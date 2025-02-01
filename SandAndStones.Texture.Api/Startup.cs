using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.Infrastructure.Handlers.UseCases.Texture;
using SandAndStones.Infrastructure;
using SandAndStones.Infrastructure.Repositories;
using SandAndStones.App.UseCases.Texture.UploadTexture;
using SandAndStones.Infrastructure.Services.Textures;
using Microsoft.Extensions.Configuration;

namespace SandAndStones.Texture.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;
        private readonly IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(_enviroment.EnvironmentName);

            var allowedOrigins = _configuration.GetSection("CorsOrigins:AllowedOrigins").Get<string[]>();
            ArgumentNullException.ThrowIfNull(allowedOrigins, nameof(allowedOrigins));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "TextureApiCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins(allowedOrigins)
                                           .AllowAnyHeader()
                                           .AllowAnyMethod()
                                           .AllowCredentials();
                                  });
            });

            AddMediatRDependencies(services);

            services
                .AddAzureBlobStorageInfrastructure(configuration)
                .AddPresentation()
                .AddHttpContextAccessor();

            services.AddSingleton<ITextureRepository, TextureRepository>();
            services.AddHostedService<TextureSeeder>();
        }

        private void AddMediatRDependencies(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<DownloadTextureByNameQuery, DownloadTextureByNameQueryResponse>, DownloadTextureByNameQueryHandler>();
            services.AddScoped<IRequestHandler<UploadTextureCommand, UploadTextureCommandResponse>, UploadTextureCommandHandler>();
        }
    }
}
