using SandAndStones.Texture.Api;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    startup.SeedTextureDataBlob(services);
}

app.UseCors("TextureApiCorsPolicy");

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
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

    app.Logger.LogInformation("Texture.Api called for path {path}", path);

    await next();
});

app.Run();