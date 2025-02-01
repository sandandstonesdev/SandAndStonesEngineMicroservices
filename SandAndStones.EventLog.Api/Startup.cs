using Azure.Messaging.EventGrid.SystemEvents;
using SandAndStones.Infrastructure;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.JsonSerialization;

namespace SandAndStones.EventLog.Api
{
    public class Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
    {
        private readonly IWebHostEnvironment _enviroment = enviroment;
        private readonly IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine($"EnvironmentName: {_enviroment.EnvironmentName}");

            var allowedOrigins = _configuration.GetSection("CorsOrigins:AllowedOrigins").Get<string[]>();
            ArgumentNullException.ThrowIfNull(allowedOrigins, nameof(allowedOrigins));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "EventLogApiCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins(allowedOrigins)
                                           .AllowAnyHeader()
                                           .AllowAnyMethod()
                                           .AllowCredentials();
                                  });
            });

            services.AddScoped(
                x => JsonSerializerService<SubscriptionValidationEventData>.Create(JsonSerializerServiceOptions.GeneralOptions));
            services.AddScoped(
                x => JsonSerializerService<EventItem>.Create(JsonSerializerServiceOptions.EventItemOptions));

            //services.AddScoped<IMediator, Mediator>();

            //services.AddScoped<IRequestHandler<GetInputAssetBatchByIdQuery, GetInputAssetBatchByIdQueryResponse>, GetInputAssetBatchByIdQueryHandler>();

            services
                .AddPresentation()
                .AddHttpContextAccessor()
                .AddConsumerInfrastructure(_configuration);
        }
    }
}
