using Microsoft.EntityFrameworkCore;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Models;

namespace SandAndStones.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);

            builder.Services.AddAuthorization();

            builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityCore<ApplicationUser>(options => {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
                //dbContext.Database.EnsureCreated();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapIdentityApi<ApplicationUser>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
