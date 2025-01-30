using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.Contracts.Services;
using SandAndStones.App.UseCases.AssetBatches.GetInputAssetBatchById;
using SandAndStones.Infrastructure;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Handlers.UseCases.AssetBatches;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Models.Assets;
using SandAndStones.Infrastructure.Repositories;
using SandAndStones.Infrastructure.Services.Assets;
using SandAndStones.Infrastructure.Services.Auth;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.Asset.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<ITokenReaderService, TokenReaderService>();

            services.AddScoped(
                _ => JsonSerializerService<InputAssetBatch>.Create(JsonSerializerServiceOptions.GeneralOptions));
            
            services.AddSingleton(new BatchReaderConfig());
            services.AddTransient<IAsyncAssetReader, InputAssetReader>();
            services.AddScoped<IInputAssetBatchRepository, InputAssetBatchRepository>();
            
            Console.WriteLine(_enviroment.IsDevelopment() ? "Development" : "Production");

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AssetApiCorsPolicy",
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

            services.AddScoped(
                x => JsonSerializerService<EventItem>.Create(JsonSerializerServiceOptions.EventItemOptions));


            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>, GetInputAssetBatchByIdQueryHandler>();

            services
                .AddPresentation()
                .AddHttpContextAccessor()
                .AddProducerInfrastructure(configuration);
        }
    }
}
