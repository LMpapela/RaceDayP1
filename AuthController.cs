
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Net.Mail;

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
        public AuthController(
            RaceDayDbContext context,
            PasswordService passwordService,
            JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }
        // End of AuthController constructor

        // Beginning of Register method
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Registration details are required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    message = "First name, last name, email and password are required."
                });
            }

            string email = request.Email.Trim().ToLowerInvariant();

            if (!MailAddress.TryCreate(email, out var emailAddress) ||
                !string.Equals(
                    emailAddress.Address,
                    email,
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "Please provide a valid email address."
                });
            }

            // Check for an existing account using a normalized email.
            bool existingUser = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email);

            if (existingUser)
            {
                return Conflict(new
                {
                    message = "A user with this email already exists."
                });
            }

            // Public registration always creates a Participant.
            // Users cannot choose their own role during registration.
            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = email,
                Password = _passwordService.HashPassword(request.Password),
                Role = "Participant"
            };

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // A database unique constraint can catch duplicate
                // registrations that happen at nearly the same time.
                _context.Entry(user).State = EntityState.Detached;

                bool emailNowExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == email);

                if (emailNowExists)
                {
                    return Conflict(new
                    {
                        message = "A user with this email already exists."
                    });
                }

                throw;
            }

            // Return a location pointing to the user's resource.
            return Created(
                $"/api/Users/{user.UserID}",
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
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    message = "Email and password are required."
                });
            }

            string email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            // Use the same response for unknown emails and incorrect
            // passwords so the endpoint does not reveal which accounts exist.
            if (existingUser == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            bool passwordValid = _passwordService.VerifyPassword(
                request.Password,
                existingUser.Password);

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            string token = _jwtService.GenerateToken(existingUser);

            return Ok(new
            {
                message = "Login successful.",
                token,
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
