using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SandAndStones.Infrastructure.Models;

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
            if (!await roleManager.RoleExistsAsync(UserRoles.AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.AdminRole));
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.UserRole))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.UserRole));
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
                
                var result = await _userManager.CreateAsync(administrator, hashedPass);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(administrator, UserRoles.UserRole);
                    await _userManager.AddToRoleAsync(administrator, UserRoles.AdminRole);
                }
            }
        }

    }
}