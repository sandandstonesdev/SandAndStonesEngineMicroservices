using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SandAndStones.Api.UseCases.User.LoginUser;
using SandAndStones.Infrastructure;

namespace SandAndStones.App
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddInfrastructure();
            return services;
        }

        public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));


            //services.AddMediatR(options =>
            //{
            //    options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            //options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            //options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //s});

            //services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
            return services;
        }
    }
}
