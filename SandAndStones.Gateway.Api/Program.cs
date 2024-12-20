using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SandAndStones.Domain.Constants;
using SandAndStones.Gateway.Api;
using SandAndStones.Infrastructure.Configuration;
using SandAndStones.Infrastructure.Data;
using SandAndStones.Infrastructure.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? default!;
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(jwtSettings.RefreshTokenProviderName);

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromSeconds(jwtSettings.RefreshTokenExpireSeconds);
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAuthenticatedUserPolicy",
        policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Role, UserRoles.UserRole)
);

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
        ClockSkew = TimeSpan.Zero
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

builder.Services.AddScoped<ApplicationDbContextConfigurator>();

builder.Services.AddControllers();

var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");

builder.Services.AddReverseProxy()
    .LoadFromConfig(reverseProxyConfig)
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(transformContext =>
        {
            transformContext.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);

            if (accessToken != null)
            {
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return ValueTask.CompletedTask;
        });
    });

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    using var scope = app.Services.CreateScope();
    var dbContextConfigurator = scope.ServiceProvider.GetRequiredService<ApplicationDbContextConfigurator>();
    await dbContextConfigurator.InitAsync();
    await dbContextConfigurator.SeedAsync();
}

app.UseCors("GatewayApiCorsPolicy");

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

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value;
        
        app.Logger.LogInformation("Gateway called for path {path}", path);
        await next();
    });
}).RequireAuthorization("RequireAuthenticatedUserPolicy");

app.Run();
