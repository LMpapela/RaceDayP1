using System.Security.Cryptography;

// Beginning of Password service namespace
namespace RaceDayP1.Services
{
    // Beginning of PasswordService class
    public class PasswordService
    {
        // Beginning of HashPassword method
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
        // End of HashPassword method

        // Beginning of VerifyPassword method
        public bool VerifyPassword(string password, string storedPassword)
        {
            var parts = storedPassword.Split(':');

            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(
                hash,
                storedHash);
        }
        // End of VerifyPassword method
    }
    // End of PasswordService class
}
// End of Password service namespace