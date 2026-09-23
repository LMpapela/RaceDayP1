using System.ComponentModel.DataAnnotations;

// Beginning of LoginRequest model namespace
namespace RaceDayP1.Models
{
    // Beginning of LoginRequest class
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
    // End of LoginRequest class
}
// End of LoginRequest model namespace