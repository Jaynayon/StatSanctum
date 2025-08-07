using System.Security.Cryptography;
using System.Text;

namespace StatSanctum.API.Helper
{
    public static class PasswordHelper
    {
        public static string GenerateSalt(int length = 16)
        {
            using var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[length];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("Password and salt cannot be null or empty.");
            }
            using var sha256 = SHA256.Create();
            var saltedPassword = $"{password}{salt}";
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
