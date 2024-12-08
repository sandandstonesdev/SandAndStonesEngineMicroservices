using Microsoft.OpenApi.Models;

namespace SandAndStones.Asset.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "Asset Api", Version = "v1" });
                config.CustomSchemaIds(type => type.FullName);
            });

            services.AddProblemDetails();

            return services;
        }
    }
}
