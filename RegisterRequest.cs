using System.ComponentModel.DataAnnotations;

// Beginning of RegisterRequest model namespace
namespace RaceDayP1.Models
{
    // Beginning of RegisterRequest class
    public class RegisterRequest
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
    }
    // End of RegisterRequest class
}
// End of RegisterRequest model namespace