using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMongoDbInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new MongoDbSettings();
            var mongoConfig = configuration.GetSection(nameof(MongoDbSettings));
            mongoConfig.Bind(settings);

            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);

            services.AddSingleton(mongoDatabase);

            services.AddSingleton<IMongoDbEventLogService>(resolver =>
                new MongoDbEventLogService(settings.ConnectionString, mongoDatabase));

            return services;
        }

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

        public static IServiceCollection AddKafkaInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProducerService>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<KafkaProducerService>>();

                var producerSettings = new KafkaProducerSettings();
                var producerConfig = configuration.GetSection(nameof(KafkaProducerSettings));
                producerConfig.Bind(producerSettings);

                return new KafkaProducerService(producerSettings, logger);
            });

            return services;
        }

        public static IServiceCollection AddKafkaConsumerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbInfrastructure(configuration);

            services.AddSingleton<IConsumerService>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<KafkaConsumerService>>();

                var consumerSettings = new KafkaConsumerSettings();
                var consumerConfig = configuration.GetSection(nameof(KafkaConsumerSettings));
                consumerConfig.Bind(consumerSettings);

                var mongoDbEventLogService = resolver.GetRequiredService<IMongoDbEventLogService>();

                return new KafkaConsumerService(mongoDbEventLogService, consumerSettings, logger);
            });

            services.AddHostedService(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<IConsumerService>();
            });

            return services;
        }
    }
}
