using System.Security.Claims;

namespace SandAndStones.Infrastructure.Contracts
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userId, IList<string> userRoles, string email);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
