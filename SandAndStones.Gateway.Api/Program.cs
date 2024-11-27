using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SandAndStones.Domain.Constants;
using SandAndStones.Gateway.Api;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? default!;
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.FromSeconds(0)
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            ctx.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);
            if (!string.IsNullOrEmpty(accessToken))
                ctx.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddSignInManager()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(jwtSettings.RefreshTokenProviderName);

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromSeconds(jwtSettings.RefreshTokenExpireSeconds);
});

builder.Services.AddScoped<ApplicationDbContextConfigurator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    using var scope = app.Services.CreateScope();
    var dbContextConfigurator = scope.ServiceProvider.GetRequiredService<ApplicationDbContextConfigurator>();
    await dbContextConfigurator.InitAsync();
    await dbContextConfigurator.SeedAsync();
}

app.UseCors("ApiCorsPolicy");

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapIdentityApi<ApplicationUser>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value;
        
        app.Logger.LogInformation("Gateway called for path {path}", path);
        await next();
    });
}
);

app.Run();