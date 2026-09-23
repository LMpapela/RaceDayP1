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
            return await _context.Users.ToListAsync();
        }
        // End of GetAllUsers method

        // Beginning of GetUserById method
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        // End of GetUserById method

        // Beginning of CreateUser method
        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }
        // End of CreateUser method

        // Beginning of UpdateUser method
        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserID);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();

            return true;
        }
        // End of UpdateUser method

        // Beginning of DeleteUser method
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
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