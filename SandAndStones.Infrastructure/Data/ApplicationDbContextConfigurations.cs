using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Models;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Data
{
    public class ApplicationDbContextConfigurator(
        ILogger<ApplicationDbContextConfigurator> logger,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IPasswordHasher<ApplicationUser> passwordHasher)
    {
        private readonly ILogger<ApplicationDbContextConfigurator> _logger = logger;
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;

        public async Task InitAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            var administratorRole = new IdentityRole("Administrator");

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                var role = await _roleManager.CreateAsync(administratorRole);
                if (role != null)
                {
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleView"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleAdd"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleEdit"));
                    await _roleManager.AddClaimAsync(administratorRole, new Claim("RoleClaim", "HasRoleDelete"));
                }
            }

            var administrator = new ApplicationUser { UserName = "sandandstones@sandandstones.com", Email = "sandandstones@sandandstones.com" };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                var pass = _configuration["DBAdminPass"];
                if (string.IsNullOrWhiteSpace(pass))
                {
                    return;
                }

                var hashedPass = _passwordHasher.HashPassword(administrator, pass);

                await _userManager.CreateAsync(administrator, hashedPass);
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
                }
            }
        }

    }
}