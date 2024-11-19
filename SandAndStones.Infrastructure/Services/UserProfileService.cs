using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Models;
using System.Net;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public class UserProfileService(
        ITokenGenerator tokenGenerator,
        UserManager<ApplicationUser> userManager) : IUserProfileService
    {
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<UserProfile> GetUserProfile(HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);

            var principal = _tokenGenerator.GetPrincipalFromToken(accessToken);
            ArgumentNullException.ThrowIfNull(principal, nameof(principal));
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(email, "email.Value");

            var user = await _userManager.FindByEmailAsync(email);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var remoteIpAddress = httpContext.Connection.RemoteIpAddress.ToString();

            return new UserProfile(
                user.Email,
                user.UserName,
                "123456789",
                string.Empty,
                "Level",
                DateTime.Now,
                DateTime.Now,
                remoteIpAddress,
                []
                );
        }
    }
}
