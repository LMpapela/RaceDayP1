using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Result service namespace
namespace RaceDayP1.Services
{
    // Beginning of ResultService class
    public class ResultService
    {
        private readonly RaceDayDbContext _context;

        // Beginning of ResultService constructor
        public ResultService(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of ResultService constructor

        // Beginning of GetAllResults method
        public async Task<List<Result>> GetAllResults()
        {
            return await _context.Results.ToListAsync();
        }
        // End of GetAllResults method

        // Beginning of GetResultsForUser method
        public async Task<List<Result>> GetResultsForUser(int userId)
        {
            return await _context.Results
                .Join(
                    _context.Enrolments,
                    result => result.EnrolmentID,
                    enrolment => enrolment.EnrolmentID,
                    (result, enrolment) => new
                    {
                        Result = result,
                        UserID = enrolment.UserID
                    })
                .Where(item => item.UserID == userId)
                .Select(item => item.Result)
                .ToListAsync();
        }
        // End of GetResultsForUser method

        // Beginning of GetResultById method
        public async Task<Result?> GetResultById(int id)
        {
            return await _context.Results.FindAsync(id);
        }
        // End of GetResultById method

        // Beginning of GetResultByIdForUser method
        public async Task<Result?> GetResultByIdForUser(
            int id,
            int userId)
        {
            return await _context.Results
                .Join(
                    _context.Enrolments,
                    result => result.EnrolmentID,
                    enrolment => enrolment.EnrolmentID,
                    (result, enrolment) => new
                    {
                        Result = result,
                        UserID = enrolment.UserID
                    })
                .Where(item =>
                    item.Result.ResultID == id &&
                    item.UserID == userId)
                .Select(item => item.Result)
                .FirstOrDefaultAsync();
        }
        // End of GetResultByIdForUser method

        // Beginning of CreateResult method
        public async Task<(bool Success, string Message, Result? Result)> CreateResult(
            Result result)
        {
            // Check that the enrolment exists
            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == result.EnrolmentID);

            if (!enrolmentExists)
            {
                return (
                    false,
                    "The selected enrolment does not exist.",
                    null);
            }

            // Check that the enrolment does not already have a result
            var existingResult = await _context.Results
                .AnyAsync(r => r.EnrolmentID == result.EnrolmentID);

            if (existingResult)
            {
                return (
                    false,
                    "A result already exists for this enrolment.",
                    null);
            }

            // Check that the finishing position is valid
            if (result.FinishPosition <= 0)
            {
                return (
                    false,
                    "Finish position must be greater than zero.",
                    null);
            }

            // Check that the finish time is valid
            if (result.FinishTime <= TimeSpan.Zero)
            {
                return (
                    false,
                    "Finish time must be greater than zero.",
                    null);
            }

            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return (
                true,
                "Result created successfully.",
                result);
        }
        // End of CreateResult method

        // Beginning of UpdateResult method
        public async Task<(bool Success, string Message)> UpdateResult(
            Result result)
        {
            var existingResult =
                await _context.Results.FindAsync(result.ResultID);

            if (existingResult == null)
            {
                return (
                    false,
                    "The result does not exist.");
            }

            // Check that the new enrolment exists
            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == result.EnrolmentID);

            if (!enrolmentExists)
            {
                return (
                    false,
                    "The selected enrolment does not exist.");
            }

            // Check that another result does not already use the enrolment
            var duplicateResult = await _context.Results
                .AnyAsync(r =>
                    r.EnrolmentID == result.EnrolmentID &&
                    r.ResultID != result.ResultID);

            if (duplicateResult)
            {
                return (
                    false,
                    "A result already exists for this enrolment.");
            }

            // Check that the finishing position is valid
            if (result.FinishPosition <= 0)
            {
                return (
                    false,
                    "Finish position must be greater than zero.");
            }

            // Check that the finish time is valid
            if (result.FinishTime <= TimeSpan.Zero)
            {
                return (
                    false,
                    "Finish time must be greater than zero.");
            }

            existingResult.EnrolmentID = result.EnrolmentID;
            existingResult.FinishPosition = result.FinishPosition;
            existingResult.FinishTime = result.FinishTime;

            await _context.SaveChangesAsync();

            return (
                true,
                "Result updated successfully.");
        }
        // End of UpdateResult method

        // Beginning of GetPerformanceHistory method
        public async Task<List<Result>> GetPerformanceHistory(int userId)
        {
            return await _context.Results
                .Join(
                    _context.Enrolments,
                    result => result.EnrolmentID,
                    enrolment => enrolment.EnrolmentID,
                    (result, enrolment) => new
                    {
                        Result = result,
                        UserID = enrolment.UserID
                    })
                .Where(item => item.UserID == userId)
                .OrderByDescending(item => item.Result.FinishPosition)
                .Select(item => item.Result)
                .ToListAsync();
        }
        // End of GetPerformanceHistory method
    }
    // End of ResultService class
}
// End of Result service namespace