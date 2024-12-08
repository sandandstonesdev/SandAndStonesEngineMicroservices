using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.Texture.DownloadTextureByName;
using SandAndStones.App.UseCases.Texture.GetTextureById;
using SandAndStones.App.UseCases.Texture.GetTexturesDecriptions;
using SandAndStones.Infrastructure.Handlers.UseCases.Texture;
using SandAndStones.Infrastructure.Repositories;

namespace SandAndStones.Texture.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            var textureRepository = new InputTextureRepository();
            textureRepository.Init();
            services.AddSingleton<IInputTextureRepository>(textureRepository);

            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");

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

            services
                .AddPresentation()
                .AddHttpContextAccessor();
        }
    }
}
