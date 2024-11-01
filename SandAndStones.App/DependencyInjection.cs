using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Infrastructure;

namespace SandAndStones.App
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddInfrastructure(configuration);
            return services;
        }

        public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                //options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                //options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
