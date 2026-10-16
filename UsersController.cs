
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Security.Claims;

// Beginning of Users controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of UsersController class
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PasswordService _passwordService;

        // Beginning of UsersController constructor
        public UsersController(
            UserService userService,
            PasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }
        // End of UsersController constructor

        // Beginning of GetUsers method
        [HttpGet]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();

            var safeUsers = users.Select(user => new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });

            return Ok(safeUsers);
        }
        // End of GetUsers method

        // Beginning of GetUser method
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(int id)
        {
            var currentUserIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            bool isOrganiser = User.IsInRole("Organiser");
            bool isParticipant = User.IsInRole("Participant");

            // Only organisers and participants can access profiles.
            if (!isOrganiser && !isParticipant)
            {
                return Forbid();
            }

            // Participants can view only their own profile.
            if (isParticipant && !isOrganiser && currentUserId != id)
            {
                return Forbid();
            }

            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var safeUser = new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            };

            return Ok(safeUser);
        }
        // End of GetUser method

        // Beginning of CreateUser method
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<object>> CreateUser(
            [FromBody] CreateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "User details are required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "First name, last name, email and password are required."
                });
            }

            string role = request.Role?.Trim() ?? string.Empty;

            if (!string.Equals(role, "Participant",
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(role, "Organiser",
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "Role must be Participant or Organiser."
                });
            }

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                Password = _passwordService.HashPassword(request.Password),
                Role = string.Equals(role, "Organiser",
                    StringComparison.OrdinalIgnoreCase)
                    ? "Organiser"
                    : "Participant"
            };

            try
            {
                var createdUser = await _userService.CreateUser(user);

                var safeUser = new
                {
                    createdUser.UserID,
                    createdUser.FirstName,
                    createdUser.LastName,
                    createdUser.Email,
                    createdUser.Role
                };

                return CreatedAtAction(
                    nameof(GetUser),
                    new { id = createdUser.UserID },
                    safeUser);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // End of CreateUser method

        // Beginning of UpdateUser method
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Profile details are required."
                });
            }

            var currentUserIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            bool isOrganiser = User.IsInRole("Organiser");
            bool isParticipant = User.IsInRole("Participant");

            if (!isOrganiser && !isParticipant)
            {
                return Forbid();
            }

            // Participants can update only their own profiles.
            if (isParticipant && !isOrganiser && currentUserId != id)
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new
                {
                    message = "First name, last name and email are required."
                });
            }

            var existingUser = await _userService.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Only update permitted profile fields.
            // Password and role are not changed by this endpoint.
            existingUser.FirstName = request.FirstName.Trim();
            existingUser.LastName = request.LastName.Trim();
            existingUser.Email = request.Email.Trim();

            try
            {
                var updated = await _userService.UpdateUser(existingUser);

                if (!updated)
                {
                    return NotFound(new { message = "User not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // End of UpdateUser method

        // Beginning of DeleteUser method
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUser(id);

                if (!deleted)
                {
                    return NotFound(new { message = "User not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // End of DeleteUser method
    }
    // End of UsersController class
}
// End of Users controller namespace
