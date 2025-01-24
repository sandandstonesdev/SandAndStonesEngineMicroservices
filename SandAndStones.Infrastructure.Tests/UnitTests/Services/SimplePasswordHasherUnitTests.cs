using Microsoft.AspNetCore.Identity;
using Moq;
using SandAndStones.Infrastructure.Models;
using SandAndStones.Infrastructure.Services.Auth;

namespace SandAndStones.Infrastructure.Tests.UnitTests.Services
{
    public class SimplePasswordHasherUnitTests
    {
        private readonly SimplePasswordHasher _passwordHasher;
        private readonly Mock<ApplicationUser> _mockUser;

        public SimplePasswordHasherUnitTests()
        {
            var salt = "1234";
            _passwordHasher = new SimplePasswordHasher(salt);
            _mockUser = new Mock<ApplicationUser>();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var providedPassword = "abcdef12345";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(_mockUser.Object, providedPassword);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(providedPassword, hashedPassword);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnSuccess_WhenPasswordsMatch()
        {
            // Arrange
            var providedPassword = "abcdef12345";
            var hashedPassword = _passwordHasher.HashPassword(_mockUser.Object, providedPassword);

            // Act
            var result = _passwordHasher.VerifyHashedPassword(_mockUser.Object, hashedPassword, providedPassword);

            // Assert
            Assert.Equal(PasswordVerificationResult.Success, result);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnFailed_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var providedPassword = "abcdef12345";
            var wrongPassword = "wrongpassword";
            var hashedPassword = _passwordHasher.HashPassword(_mockUser.Object, providedPassword);

            // Act
            var result = _passwordHasher.VerifyHashedPassword(_mockUser.Object, hashedPassword, wrongPassword);

            // Assert
            Assert.Equal(PasswordVerificationResult.Failed, result);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashes_ForDifferentPasswords()
        {
            // Arrange
            var password1 = "password1";
            var password2 = "password2";

            // Act
            var hashedPassword1 = _passwordHasher.HashPassword(_mockUser.Object, password1);
            var hashedPassword2 = _passwordHasher.HashPassword(_mockUser.Object, password2);

            // Assert
            Assert.NotEqual(hashedPassword1, hashedPassword2);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashes_ForSamePasswordWithDifferentSalts()
        {
            // Arrange
            var password = "password";
            var salt1 = "salt1";
            var salt2 = "salt2";
            var passwordHasher1 = new SimplePasswordHasher(salt1);
            var passwordHasher2 = new SimplePasswordHasher(salt2);

            // Act
            var hashedPassword1 = passwordHasher1.HashPassword(_mockUser.Object, password);
            var hashedPassword2 = passwordHasher2.HashPassword(_mockUser.Object, password);

            // Assert
            Assert.NotEqual(hashedPassword1, hashedPassword2);
        }

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
