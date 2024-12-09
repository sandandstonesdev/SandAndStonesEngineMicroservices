using Microsoft.OpenApi.Models;
using SandAndStones.App.Contracts.Repository;
using SandAndStones.Infrastructure;
using SandAndStones.Infrastructure.Repositories;

namespace SandAndStones.Gateway.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthInfrastructure(configuration);
            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "Gateway Api", Version = "v1" });
                config.CustomSchemaIds(type => type.FullName);
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(
                    new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });
            services.AddProblemDetails();

            return services;
        }
    }
}
