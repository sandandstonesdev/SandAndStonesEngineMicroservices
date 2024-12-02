using SandAndStones.Domain.Constants;

namespace SandAndStones.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration, builder.Environment);
            startup.ConfigureServices(builder.Services);

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseCors("ApiCorsPolicy");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value;
                
                app.Logger.LogInformation("Api called for path {path}", path);
                
                await next();
            });

            app.Run();
        }
    }
}
