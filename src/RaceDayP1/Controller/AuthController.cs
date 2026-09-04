using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of authentication controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of AuthController class
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        // Beginning of AuthController constructor
        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of AuthController constructor

        // Beginning of Register method
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "Register",
                new { id = user.UserID },
                user);
        }
        // End of Register method

        // Beginning of Login method
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == user.Email &&
                    u.Password == user.Password);

            if (existingUser == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(existingUser);
        }
        // End of Login method
    }
    // End of AuthController class
}
// End of authentication controller namespace