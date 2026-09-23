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
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
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
        public async Task<ActionResult<object>> CreateUser(CreateUserRequest request)
        {
            if (request.Role != "Participant" && request.Role != "Organiser")
            {
                return BadRequest("Role must be Participant or Organiser.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = _passwordService.HashPassword(request.Password),
                Role = request.Role
            };

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
        // End of CreateUser method

        // Beginning of UpdateUser method
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            // Retrieve the current user's ID from the claims
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
            {// If the claim is not a valid integer, return Unauthorized
                return Unauthorized();
            }// If the claim is a valid integer, continue with the update process

            // Check if the current user is an organiser
            var isOrganiser = User.IsInRole("Organiser");

            // If the current user is not an organiser and is trying to update another user's information, return Forbid
            if (!isOrganiser && currentUserId != id)
            {// If the current user is not an organiser and is trying to update another user's information, return Forbid
                return Forbid();
            }// If the current user is an organiser or is updating their own information, continue with the update process

            // Retrieve the existing user from the database
            var existingUser = await _userService.GetUserById(id);

            // If the user does not exist, return NotFound
            if (existingUser == null)
            {// If the user does not exist, return NotFound
                return NotFound();
            }// If the user exists, continue with the update process

            // Update the user's information with the new values from the request
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;

            // If the current user is an organiser, allow them to update the role
            var updated = await _userService.UpdateUser(existingUser);

            // If the update was not successful, return NotFound
            if (!updated)
            {// If the update was not successful, return NotFound
                return NotFound();
            }// If the update was successful, return NoContent


            return NoContent();
        }
        // End of UpdateUser method

        /* 
         * 
        //Request method to update using https
        [HttpPut]
        public string update_user()
        {// start of method
            return "it works";
        }// end of method
        
         */

        // Beginning of DeleteUser method
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUser(id);
            // If the user was not found, return NotFound
            if (!deleted)
            {// If the user was not found, return NotFound
                return NotFound();
            }// If the user was found and deleted, return NoContent

            // If the user was found and deleted, return NoContent
            return NoContent();
        }
        // End of DeleteUser method
    }
    // End of UsersController class
}
// End of Users controller namespace