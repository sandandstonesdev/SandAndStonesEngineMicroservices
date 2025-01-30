using Microsoft.AspNetCore.Http;
using Moq;
using SandAndStones.Domain.Constants;
using SandAndStones.Infrastructure.Services.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class TokenReaderServiceTests
    {
        [Fact]
        public void GetUserEmailFromToken_WhenCalled_ReturnsUserEmail()
        {
            // Arrange
            var accessToken = "accessToken";
            var userEmail = "userEmail";
            var jwtToken = new JwtSecurityToken(claims: new[] { new Claim(JwtRegisteredClaimNames.Email, userEmail) });

            var jwtFactoryMock = new Mock<IJwtFactory>();
            var jwtSecurityTokenHandlerMock = new Mock<JwtSecurityTokenHandler>();
            jwtSecurityTokenHandlerMock.Setup(x => x.ReadToken(accessToken)).Returns(jwtToken);

            jwtFactoryMock.Setup(x => x.CreateJwtSecurityTokenHandler()).Returns(jwtSecurityTokenHandlerMock.Object);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext.Request.Cookies.TryGetValue(JwtTokenConstants.AccessTokenName, out accessToken));

            var tokenReaderService = new TokenReaderService(jwtFactoryMock.Object, contextAccessor.Object);
            
            // Act
            var result = tokenReaderService.GetUserEmailFromToken();
            
            // Assert
            Assert.Equal(userEmail, result);
        }
    }
}
