using System.IdentityModel.Tokens.Jwt;

namespace SandAndStones.Infrastructure.Services.Auth
{
    public class JwtFactory : IJwtFactory
    {
        public JwtSecurityTokenHandler CreateJwtSecurityTokenHandler()
        {
            return new JwtSecurityTokenHandler();
        }
    }
}
