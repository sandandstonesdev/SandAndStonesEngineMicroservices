using Microsoft.AspNetCore.Identity;
using Moq;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services;

namespace SandAndStones.App.Tests
{
    public class SimplePasswordHasherUnitTest()
    {
        public class SimplePasswordHasher_Should
        {
            [Fact]
            public void SimplePasswordHasher_ShouldDoHashAndVerifyPasswordProcess_Correctly()
            {
                // Arrange
                var salt = "1234";
                var providedPassword = "abcdef12345";
                var hashedProvidedPassword = "q81K5b3PD1fVHa8yDO0lhIKNrqyfpKo/tsRHvZSnwilYbthb8QJOWu1FzlOhDoKcC3vGrz221RjshOOKJHfkxg==";
                var passwordHasher = new SimplePasswordHasher(salt);
                var user = new Mock<ApplicationUser>();
                
                // Act
                
                // Register User
                var hashedPassword = passwordHasher.HashPassword(user.Object, providedPassword);
                hashedPassword = passwordHasher.HashPassword(user.Object, hashedPassword);

                // Login User
                var result = passwordHasher.VerifyHashedPassword(user.Object, hashedPassword, hashedProvidedPassword);
                
                // Assert
                Assert.Equal(PasswordVerificationResult.Success, result);
            }
        }
    }
}
