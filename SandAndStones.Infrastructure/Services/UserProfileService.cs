using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Domain.Constants;
using SandAndStones.Domain.Entities;
using SandAndStones.Infrastructure.Contracts;
using SandAndStones.Infrastructure.Models;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public class UserProfileService(
        ITokenGenerator tokenGenerator,
        UserManager<ApplicationUser> userManager) : IUserProfileService
    {
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<UserProfile> GetUserProfile(IHttpContextAccessor contextAccessor)
        {
            contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);

            var principal = _tokenGenerator.GetPrincipalFromToken(accessToken);
            ArgumentNullException.ThrowIfNull(principal, nameof(principal));
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "email.Value");

            var claims = principal.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            var privileges = string.Join(", ", claims);

            var user = await _userManager.FindByEmailAsync(email);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Email, nameof(user.Email));
            ArgumentException.ThrowIfNullOrWhiteSpace(user.UserName, nameof(user.UserName));

            var remoteIpAddress = contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            return new UserProfile(
                user.Email,
                user.UserName,
                "123456789",
                string.Empty,
                privileges,
                DateTime.Now,
                DateTime.Now,
                remoteIpAddress,
                []
                );
        }
    }
}
