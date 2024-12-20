using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.GetTextureById;
using SandAndStones.App.UseCases.Texture.GetTexturesDecriptions;
using SandAndStones.Infrastructure.Handlers.UseCases.Texture;
using SandAndStones.Infrastructure;
using SandAndStones.Infrastructure.Repositories;
using System;
using SandAndStones.App.UseCases.Texture.UploadTexture;

namespace SandAndStones.Texture.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(_enviroment.EnvironmentName);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "TextureApiCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins(
                                            "https://localhost:5000",
                                            "https://sand-and-stones-gateway-0001.azurewebsites.net")
                                           .AllowAnyHeader()
                                           .AllowAnyMethod()
                                           .AllowCredentials();
                                  });
            });

            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<GetTextureByIdQuery, GetTextureByIdQueryResponse>, GetTextureByIdQueryHandler>();
            services.AddScoped<IRequestHandler<DownloadTextureByNameQuery, DownloadTextureByNameQueryResponse>, DownloadTextureByNameQueryHandler>();
            services.AddScoped<IRequestHandler<GetTexturesDescriptionsQuery, GetTexturesDescriptionsQueryResponse>, GetTexturesDescriptionsQueryHandler>();
            services.AddScoped<IRequestHandler<UploadTextureCommand, UploadTextureCommandResponse>, UploadTextureCommandHandler>();

            services
                .AddAzureBlobStorageInfrastructure(configuration)
                .AddPresentation()
                .AddHttpContextAccessor();

            services.AddSingleton<IInputTextureRepository, InputTextureRepository>();
        }

        public void SeedTextureDataBlob(IServiceProvider serviceProvider)
        {
            var textureRepository = serviceProvider.GetRequiredService<IInputTextureRepository>();
            var initializeTask = textureRepository.SeedTextures();
            initializeTask.GetAwaiter().GetResult();
        }
    }
}
