using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            var salt = configuration["HasherModule:TestSalt"] ?? "1234";

            var passwordHasher = new SimplePasswordHasher(salt);
            services.AddTransient<IPasswordHasher<ApplicationUser>, SimplePasswordHasher>(_ => passwordHasher);
            return services;
        }
    }
}
