using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;
using RaceDayP1.Services;

// Beginning of authentication controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of AuthController class
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        // Beginning of AuthController constructor
        public AuthController(RaceDayDbContext context,PasswordService passwordService,JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }
        // End of AuthController constructor

        // Beginning of Register method
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = _passwordService.HashPassword(request.Password),
                Role = "Participant"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Register),
                new { id = user.UserID },
                new
                {
                    message = "Registration successful.",
                    user = new
                    {
                        user.UserID,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Role
                    }
                });
        }
        // End of Register method

        // Beginning of Login method
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var passwordValid = _passwordService.VerifyPassword(request.Password,existingUser.Password);

            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(existingUser);

            return Ok(new
            {
                message = "Login successful.",
                token = token,
                user = new
                {
                    existingUser.UserID,
                    existingUser.FirstName,
                    existingUser.LastName,
                    existingUser.Email,
                    existingUser.Role
                }
            });
        }
        // End of Login method
    }
    // End of AuthController class
}
// End of authentication controller namespace