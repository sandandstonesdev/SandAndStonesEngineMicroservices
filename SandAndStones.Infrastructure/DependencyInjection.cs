using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            var salt = configuration["HasherModule:TestSalt"] ?? throw new ArgumentException("HasherModule:TestSalt config missing");

            var passwordHasher = new SimplePasswordHasher(salt);
            services.AddTransient<IPasswordHasher<ApplicationUser>, SimplePasswordHasher>(_ => passwordHasher);
            return services;
        }
    }
}
