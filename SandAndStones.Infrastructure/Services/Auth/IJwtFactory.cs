using System.IdentityModel.Tokens.Jwt;

namespace SandAndStones.Infrastructure.Services.Auth
{
    public interface IJwtFactory
    {
        JwtSecurityTokenHandler CreateJwtSecurityTokenHandler();
    }
}