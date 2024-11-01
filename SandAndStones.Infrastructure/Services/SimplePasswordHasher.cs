using Microsoft.AspNetCore.Identity;
using SandAndStones.Infrastructure.Models;
using System.Security.Cryptography;
using System.Text;

namespace SandAndStones.Infrastructure.Services
{
    public class SimplePasswordHasher(string salt) : IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashedPassword = Convert.ToBase64String(sha512.ComputeHash(saltedPasswordAsBytes));
                return hashedPassword;
            }
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(user, providedPassword))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
    }

}
