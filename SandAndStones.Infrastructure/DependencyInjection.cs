using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();

            return services;
        }
    }
}
