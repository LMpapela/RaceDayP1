// Beginning of User model namespace
namespace RaceDayP1.Models
{
    // Beginning of User class
    public class User
    {
        public int UserID { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
    // End of User class
}
// End of User model namespace