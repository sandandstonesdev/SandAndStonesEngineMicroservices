using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SandAndStones.App
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureMediatR(this IServiceCollection services, List<Assembly>? commandAssemblies=default)
        {
            commandAssemblies ??= [];

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly(),.. commandAssemblies]);
                
                //options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                //options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            //services.AddScoped<IRequestHandler<LoginUserRequest, LoginUserResponse>, LoginUserCommand>();

            return services;
        }
    }
}
