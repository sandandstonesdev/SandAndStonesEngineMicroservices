using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Infrastructure;
using System.Reflection;

namespace SandAndStones.App
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGatewayDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthInfrastructure(configuration);
            return services;
        }

        public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            return ConfigureMediatR(services, [Assembly.GetExecutingAssembly()]);
        }
        public static IServiceCollection ConfigureMediatR(this IServiceCollection services, Assembly[] commandAssemblies)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(commandAssemblies);
                
                //services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
                
                //options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                //options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            //services.AddScoped<IRequestHandler<LoginUserRequest, LoginUserResponse>, LoginUserCommand>();

            return services;
        }
    }
}
