using SandAndStones.Infrastructure.Models;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Services
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(ApplicationUser user, string email);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
