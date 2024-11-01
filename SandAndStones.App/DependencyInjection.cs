using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Infrastructure;

namespace SandAndStones.App
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddInfrastructure();
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
