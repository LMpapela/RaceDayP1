using System.ComponentModel.DataAnnotations;

// Beginning of CreateUserRequest model namespace
namespace RaceDayP1.Models
{
    // Beginning of CreateUserRequest class
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;
    }
    // End of CreateUserRequest class
}
// End of CreateUserRequest model namespace