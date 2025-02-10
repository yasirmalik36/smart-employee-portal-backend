using Microsoft.AspNetCore.Identity;

namespace SmartEmpAPI.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string plainPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, plainPassword);
        }

        public static bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
