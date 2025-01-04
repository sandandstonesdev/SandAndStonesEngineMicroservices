using Microsoft.AspNetCore.Http;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace SandAndStones.Infrastructure.Services
{
    public class TokenReaderService(IHttpContextAccessor contextAccessor) : ITokenReaderService
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public string GetUserEmailFromToken()
        {
            _contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            if (jwtToken == null)
                return string.Empty;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email);
            return userIdClaim?.Value ?? string.Empty;
        }
    }
}
