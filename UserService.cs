
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of User service namespace
namespace RaceDayP1.Services
{
    // Beginning of UserService class
    public class UserService
    {
        private readonly RaceDayDbContext _context;

        // Beginning of UserService constructor
        public UserService(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of UserService constructor

        // Beginning of GetAllUsers method
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }
        // End of GetAllUsers method

        // Beginning of GetUserById method
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserID == id);
        }
        // End of GetUserById method

        // Beginning of CreateUser method
        public async Task<User> CreateUser(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException(
                    "User details are required.");
            }

            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new InvalidOperationException(
                    "First name, last name, email and password are required.");
            }

            // Normalize profile fields before saving.
            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.Email = user.Email.Trim().ToLowerInvariant();

            // Prevent duplicate email addresses.
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == user.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A user with this email address already exists.");
            }

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }
        // End of CreateUser method

        // Beginning of UpdateUser method
        public async Task<bool> UpdateUser(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException(
                    "User details are required.");
            }

            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException(
                    "First name, last name and email are required.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == user.UserID);

            if (existingUser == null)
            {
                return false;
            }

            string normalizedEmail =
                user.Email.Trim().ToLowerInvariant();

            // Check whether another user already uses this email.
            bool emailExists = await _context.Users.AnyAsync(
                u => u.Email.ToLower() == normalizedEmail &&
                     u.UserID != user.UserID);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Another user already uses this email address.");
            }

            // Update profile fields only.
            // Password and role changes are not allowed here.
            existingUser.FirstName = user.FirstName.Trim();
            existingUser.LastName = user.LastName.Trim();
            existingUser.Email = normalizedEmail;

            await _context.SaveChangesAsync();

            return true;
        }
        // End of UpdateUser method

        // Beginning of DeleteUser method
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                return false;
            }

            // Prevent deletion when related records depend on this user.
            bool hasEnrolments = await _context.Enrolments
                .AnyAsync(e => e.UserID == id);

            bool organisesEvents = await _context.Events
                .AnyAsync(e => e.OrganiserID == id);

            if (hasEnrolments || organisesEvents)
            {
                throw new InvalidOperationException(
                    "This user cannot be deleted because they have " +
                    "related enrolments or organised events.");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }
        // End of DeleteUser method
    }
    // End of UserService class
}
// End of User service namespace
