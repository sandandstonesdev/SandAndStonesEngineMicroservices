using Microsoft.AspNetCore.Http;
using SandAndStones.App.Contracts.Services;
using SandAndStones.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace SandAndStones.Infrastructure.Services.Auth
{
    public class TokenReaderService(IJwtFactory jwtFactory, IHttpContextAccessor contextAccessor) : ITokenReaderService
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IJwtFactory _jwtFactory = jwtFactory;


        public string GetUserEmailFromToken()
        {
            _contextAccessor.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out var accessToken);

            var handler = _jwtFactory.CreateJwtSecurityTokenHandler();

            if (handler.ReadToken(accessToken) is not JwtSecurityToken jwtToken)
                return string.Empty;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email);
            return userIdClaim?.Value ?? string.Empty;
        }

        
    }
}
