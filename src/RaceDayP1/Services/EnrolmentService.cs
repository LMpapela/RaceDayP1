using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Enrolment service namespace
namespace RaceDayP1.Services
{
    // Beginning of EnrolmentService class
    public class EnrolmentService
    {
        private readonly RaceDayDbContext _context;

        // Beginning of EnrolmentService constructor
        public EnrolmentService(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of EnrolmentService constructor

        // Beginning of GetAllEnrolments method
        public async Task<List<Enrolment>> GetAllEnrolments()
        {
            return await _context.Enrolments.ToListAsync();
        }
        // End of GetAllEnrolments method

        // Beginning of GetEnrolmentsForUser method
        public async Task<List<Enrolment>> GetEnrolmentsForUser(int userId)
        {
            return await _context.Enrolments
                .Where(e => e.UserID == userId)
                .ToListAsync();
        }
        // End of GetEnrolmentsForUser method

        // Beginning of GetEnrolmentById method
        public async Task<Enrolment?> GetEnrolmentById(int id)
        {
            return await _context.Enrolments.FindAsync(id);
        }
        // End of GetEnrolmentById method

        // Beginning of GetEnrolmentByIdForUser method
        public async Task<Enrolment?> GetEnrolmentByIdForUser(
            int id,
            int userId)
        {
            return await _context.Enrolments
                .FirstOrDefaultAsync(e =>
                    e.EnrolmentID == id &&
                    e.UserID == userId);
        }
        // End of GetEnrolmentByIdForUser method

        // Beginning of CreateEnrolment method
        public async Task<(bool Success, string Message, Enrolment? Enrolment)> CreateEnrolment(
            Enrolment enrolment)
        {
            // Check that the user exists
            var userExists = await _context.Users
                .AnyAsync(u => u.UserID == enrolment.UserID);

            if (!userExists)
            {
                return (
                    false,
                    "The selected user does not exist.",
                    null);
            }

            // Check that the event exists
            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == enrolment.EventID);

            if (!eventExists)
            {
                return (
                    false,
                    "The selected event does not exist.",
                    null);
            }

            // Check that the category exists
            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.CategoryID == enrolment.CategoryID);

            if (category == null)
            {
                return (
                    false,
                    "The selected category does not exist.",
                    null);
            }

            // Check that the category belongs to the selected event
            if (category.EventID != enrolment.EventID)
            {
                return (
                    false,
                    "The selected category does not belong to the selected event.",
                    null);
            }

            // Check for an existing enrolment
            var existingEnrolment = await _context.Enrolments
                .AnyAsync(e =>
                    e.UserID == enrolment.UserID &&
                    e.EventID == enrolment.EventID &&
                    e.CategoryID == enrolment.CategoryID);

            if (existingEnrolment)
            {
                return (
                    false,
                    "The participant is already enrolled in this event category.",
                    null);
            }

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            return (
                true,
                "Enrolment created successfully.",
                enrolment);
        }
        // End of CreateEnrolment method

        // Beginning of DeleteEnrolment method
        public async Task<bool> DeleteEnrolment(int id)
        {
            var enrolment =
                await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
            {
                return false;
            }

            _context.Enrolments.Remove(enrolment);

            await _context.SaveChangesAsync();

            return true;
        }
        // End of DeleteEnrolment method
    }
    // End of EnrolmentService class
}
// End of Enrolment service namespace