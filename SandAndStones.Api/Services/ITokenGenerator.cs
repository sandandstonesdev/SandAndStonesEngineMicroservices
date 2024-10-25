using System.Security.Claims;

namespace SandAndStones.Api.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userId, string email);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
