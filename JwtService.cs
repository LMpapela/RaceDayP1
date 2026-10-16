
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RaceDayP1.Models;

// Beginning of JWT service namespace
namespace RaceDayP1.Services
{
    // Beginning of JwtService class
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        // Beginning of JwtService constructor
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // End of JwtService constructor

        // Beginning of GenerateToken method
        public string GenerateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    "JWT signing key is missing from configuration.");
            }

            // HS256 requires a signing key of at least 256 bits.
            if (Encoding.UTF8.GetByteCount(key) < 32)
            {
                throw new InvalidOperationException(
                    "The JWT signing key must be at least 32 bytes long.");
            }

            if (string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException(
                    "JWT issuer and audience must be configured.");
            }

            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Role))
            {
                throw new InvalidOperationException(
                    "The user must have an email address and role.");
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            // These claims are used by the API's authorization checks.
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserID.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // End of GenerateToken method
    }
    // End of JwtService class
}
// End of JWT service namespace
