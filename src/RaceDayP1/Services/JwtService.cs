using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            var key = _configuration["Jwt:Key"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // End of GenerateToken method
    }
    // End of JwtService class
}
// End of JWT service namespace