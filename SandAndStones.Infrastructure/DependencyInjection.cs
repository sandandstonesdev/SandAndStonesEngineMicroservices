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
using SandAndStones.Infrastructure.Services.Auth;
using SandAndStones.Infrastructure.Services.Bitmaps;
using SandAndStones.Infrastructure.Services.Blob;
using SandAndStones.Infrastructure.Services.EventGrid;
using SandAndStones.Infrastructure.Services.JsonSerialization;
using SandAndStones.Infrastructure.Services.Kafka;
using SandAndStones.Infrastructure.Services.Mongo;
using SandAndStones.Infrastructure.Services.Profile;
using SandAndStones.Infrastructure.Services.Textures;
using System.IO.Abstractions;

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
            services.AddTransient<IJwtFactory, JwtFactory>();
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
            services.AddSingleton<IAppContextWrapper, AppContextWrapper>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ISkiaWrapper, SkiaWrapper>();

            services.AddSingleton<IBitmapService, BitmapService>();

            var settings = new AzureBlobServiceSettings();
            var config = configuration.GetSection(nameof(AzureBlobServiceSettings));
            config.Bind(settings);
            if (settings.Enabled)
            {
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

                services.AddSingleton<ITextureFileService, AzureBlobService>();
            }
            else
            {
                services.AddSingleton<ITextureFileService, FileTextureService>();
            }
            
            
            return services;
        }

        private static IServiceCollection AddKafkaProducerInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

        private static IServiceCollection AddKafkaConsumerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbInfrastructure(configuration);

            services.AddSingleton<IConsumerService>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<KafkaConsumerService>>();

                var consumerSettings = new KafkaConsumerSettings();
                var consumerConfig = configuration.GetSection(nameof(KafkaConsumerSettings));
                consumerConfig.Bind(consumerSettings);

                var mongoDbEventLogService = resolver.GetRequiredService<IMongoDbEventLogService>();

                IJsonSerializerService<EventItem> jsonSerializerService = JsonSerializerService<EventItem>.Create(JsonSerializerServiceOptions.EventItemOptions);

                return new KafkaConsumerService(mongoDbEventLogService, jsonSerializerService, consumerSettings, logger);
            });

            services.AddHostedService(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<IConsumerService>() is not KafkaConsumerService kafkaConsumerService
                    ? throw new InvalidOperationException("Kafka consumer service is not registered")
                    : kafkaConsumerService;
            });

            return services;
        }

        private static IServiceCollection AddEventGridProducerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProducerService>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<EventGridProducerService>>();

                var producerSettings = new EventGridSettings();
                var producerConfig = configuration.GetSection(nameof(EventGridSettings));
                producerConfig.Bind(producerSettings);

                return new EventGridProducerService(producerSettings, logger);
            });

            return services;
        }

        private static IServiceCollection AddEventGridConsumerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbInfrastructure(configuration);

            services.AddSingleton<IConsumerService>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<EventGridConsumerService>>();
                var mongoDbEventLogService = resolver.GetRequiredService<IMongoDbEventLogService>();

                return new EventGridConsumerService(mongoDbEventLogService, logger);
            });

            return services;
        }

        public static IServiceCollection AddProducerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var messagingService = configuration["MessagingService"];

            if (messagingService == "Kafka")
            {
                services.AddKafkaProducerInfrastructure(configuration);
            }
            else if (messagingService == "EventGrid")
            {
                services.AddEventGridProducerInfrastructure(configuration);
            }

            return services;
        }

        public static IServiceCollection AddConsumerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var messagingService = configuration["MessagingService"];

            if (messagingService == "Kafka")
            {
                services.AddKafkaConsumerInfrastructure(configuration);
            }
            else if (messagingService == "EventGrid")
            {
                services.AddEventGridConsumerInfrastructure(configuration);
            }

            return services;
        }
    }
}
