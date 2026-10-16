
using System.Security.Cryptography;

// Beginning of Password service namespace
namespace RaceDayP1.Services
{
    // Beginning of PasswordService class
    public class PasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        // Beginning of HashPassword method
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(
                    "Password is required.",
                    nameof(password));
            }

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(HashSize);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }
        // End of HashPassword method

        // Beginning of VerifyPassword method
        public bool VerifyPassword(string password, string storedPassword)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(storedPassword))
            {
                return false;
            }

            string[] parts = storedPassword.Split(':');

            if (parts.Length != 2)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHash = Convert.FromBase64String(parts[1]);

                // Reject unexpected salt or hash sizes.
                if (salt.Length != SaltSize ||
                    storedHash.Length != HashSize)
                {
                    return false;
                }

                using var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256);

                byte[] hash = pbkdf2.GetBytes(HashSize);

                return CryptographicOperations.FixedTimeEquals(
                    hash,
                    storedHash);
            }
            catch (FormatException)
            {
                // Invalid Base64 means the stored password is malformed.
                return false;
            }
        }
        // End of VerifyPassword method
    }
    // End of PasswordService class
}
// End of Password service namespace
