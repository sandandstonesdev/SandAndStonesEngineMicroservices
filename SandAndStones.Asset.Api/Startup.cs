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
        private readonly IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<ITokenReaderService, TokenReaderService>();

            services.AddScoped(
                _ => JsonSerializerService<InputAssetBatch>.Create(JsonSerializerServiceOptions.GeneralOptions));
            
            services.AddSingleton(new BatchReaderConfig());
            services.AddTransient<IAsyncAssetReader, InputAssetReader>();
            services.AddScoped<IInputAssetBatchRepository, InputAssetBatchRepository>();

            Console.WriteLine($"EnvironmentName: {_enviroment.EnvironmentName}");

            var allowedOrigins = _configuration.GetSection("CorsOrigins:AllowedOrigins").Get<string[]>();
            ArgumentNullException.ThrowIfNull(allowedOrigins, nameof(allowedOrigins));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AssetApiCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins(allowedOrigins)
                                           .AllowAnyHeader()
                                           .AllowAnyMethod()
                                           .AllowCredentials();
                                  });
            });

            services.AddScoped(
                x => JsonSerializerService<EventItem>.Create(JsonSerializerServiceOptions.EventItemOptions));

            AddMediatRDependencies(services);

            services
                .AddPresentation()
                .AddHttpContextAccessor()
                .AddProducerInfrastructure(_configuration);
        }

        private void AddMediatRDependencies(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>, GetInputAssetBatchByIdQueryHandler>();
        }
    }
}
