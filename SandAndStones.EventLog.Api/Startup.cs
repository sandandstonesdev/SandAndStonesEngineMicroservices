using SandAndStones.Infrastructure;

namespace SandAndStones.EventLog.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine($"EnvironmentName: {_enviroment.EnvironmentName}");

            services.AddCors(options =>
            {
                options.AddPolicy(name: "EventLogApiCorsPolicy",
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

            //services.AddScoped<IMediator, Mediator>();

            //services.AddScoped<IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>, GetInputAssetBatchByIdQueryHandler>();

            services
                .AddPresentation()
                .AddHttpContextAccessor()
                .AddKafkaConsumerInfrastructure(configuration);
        }
    }
}
