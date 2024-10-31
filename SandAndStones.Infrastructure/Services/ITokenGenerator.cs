using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userId, string email);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
