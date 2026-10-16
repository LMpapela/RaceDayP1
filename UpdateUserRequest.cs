using System.ComponentModel.DataAnnotations;

// Beginning of UpdateUserRequest model namespace
namespace RaceDayP1.Models
{
    // Beginning of UpdateUserRequest class
    public class UpdateUserRequest
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
    }
    // End of UpdateUserRequest class
}
// End of UpdateUserRequest model namespace