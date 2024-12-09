using MediatR;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.App.UseCases.AssetBatch.GetInputAssetBatchById;
using SandAndStones.Infrastructure.Handlers.UseCases.AssetBatch;
using SandAndStones.Infrastructure.Repositories;

namespace SandAndStones.Asset.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInputAssetBatchRepository, InputAssetBatchRepository>();
            
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

            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>, GetInputAssetBatchByIdQueryHandler>();

            services
                .AddPresentation()
                .AddHttpContextAccessor();
        }
    }
}
