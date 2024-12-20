using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Infrastructure.Configuration;
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

        public static IServiceCollection AddAzureBlobStorageInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = new AzureBlobServiceSettings();
            var config = configuration.GetSection(nameof(AzureBlobServiceSettings));
            config.Bind(settings);

            services.AddSingleton(resolver =>
            {
                var env = resolver.GetRequiredService<IHostEnvironment>();
                
                if (env.IsDevelopment())
                {
                    return new BlobServiceClient(settings.BlobConnectionString);
                }
                else if (env.IsEnvironment("Local"))
                {
                    return new BlobServiceClient(settings.BlobConnectionString);
                }
                else
                {
                    return new BlobServiceClient(settings.BlobConnectionString);
                }
            });

            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            return services;
        }
    }
}
