
using Microsoft.AspNetCore.Http;
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
            return await _context.Results
                .AsNoTracking()
                .ToListAsync();
        }
        // End of GetAllResults method

        // Beginning of GetResultsForOrganiser method
        public async Task<List<Result>> GetResultsForOrganiser(
            int organiserId)
        {
            return await (
                from result in _context.Results.AsNoTracking()
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where eventItem.OrganiserID == organiserId
                select result
            ).ToListAsync();
        }
        // End of GetResultsForOrganiser method

        // Beginning of GetResultsForUser method
        public async Task<List<Result>> GetResultsForUser(int userId)
        {
            return await (
                from result in _context.Results.AsNoTracking()
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                where enrolment.UserID == userId
                select result
            ).ToListAsync();
        }
        // End of GetResultsForUser method

        // Beginning of GetResultById method
        public async Task<Result?> GetResultById(int id)
        {
            return await _context.Results
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ResultID == id);
        }
        // End of GetResultById method

        // Beginning of GetResultByIdForOrganiser method
        public async Task<Result?> GetResultByIdForOrganiser(
            int id,
            int organiserId)
        {
            return await (
                from result in _context.Results.AsNoTracking()
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where result.ResultID == id
                    && eventItem.OrganiserID == organiserId
                select result
            ).FirstOrDefaultAsync();
        }
        // End of GetResultByIdForOrganiser method

        // Beginning of GetResultByIdForUser method
        public async Task<Result?> GetResultByIdForUser(
            int id,
            int userId)
        {
            return await (
                from result in _context.Results.AsNoTracking()
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                where result.ResultID == id
                    && enrolment.UserID == userId
                select result
            ).FirstOrDefaultAsync();
        }
        // End of GetResultByIdForUser method

        // Beginning of CreateResult method
        public async Task<(
            bool Success,
            string Message,
            Result? Result,
            int StatusCode)> CreateResult(
                Result result,
                int organiserId)
        {
            // Check that the enrolment and its event exist
            var enrolmentEvent = await (
                from enrolment in _context.Enrolments
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where enrolment.EnrolmentID == result.EnrolmentID
                select new
                {
                    eventItem.OrganiserID
                }
            ).FirstOrDefaultAsync();

            if (enrolmentEvent == null)
            {
                return (false,
                    "The selected enrolment does not exist.",
                    null,
                    StatusCodes.Status404NotFound);
            }

            // Organisers can only record results for their own events
            if (enrolmentEvent.OrganiserID != organiserId)
            {
                return (false,
                    "You cannot manage results for another organiser's event.",
                    null,
                    StatusCodes.Status403Forbidden);
            }

            // Only one result is allowed per enrolment
            bool existingResult = await _context.Results
                .AnyAsync(r => r.EnrolmentID == result.EnrolmentID);

            if (existingResult)
            {
                return (false,
                    "A result already exists for this enrolment.",
                    null,
                    StatusCodes.Status409Conflict);
            }

            // Validate finishing position
            if (result.FinishPosition <= 0)
            {
                return (false,
                    "Finish position must be greater than zero.",
                    null,
                    StatusCodes.Status400BadRequest);
            }

            // Validate finish time
            if (result.FinishTime <= TimeSpan.Zero)
            {
                return (false,
                    "Finish time must be greater than zero.",
                    null,
                    StatusCodes.Status400BadRequest);
            }

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return (true,
                "Result created successfully.",
                result,
                StatusCodes.Status201Created);
        }
        // End of CreateResult method

        // Beginning of UpdateResult method
        public async Task<(
            bool Success,
            string Message,
            int StatusCode)> UpdateResult(
                Result result,
                int organiserId)
        {
            var existingResult = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == result.ResultID);

            if (existingResult == null)
            {
                return (false,
                    "The result does not exist.",
                    StatusCodes.Status404NotFound);
            }

            // Confirm ownership of the event attached to the existing result
            var existingOwnerId = await (
                from enrolment in _context.Enrolments
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where enrolment.EnrolmentID == existingResult.EnrolmentID
                select (int?)eventItem.OrganiserID
            ).FirstOrDefaultAsync();

            if (existingOwnerId == null)
            {
                return (false,
                    "The event associated with this result was not found.",
                    StatusCodes.Status404NotFound);
            }

            if (existingOwnerId.Value != organiserId)
            {
                return (false,
                    "You cannot update results for another organiser's event.",
                    StatusCodes.Status403Forbidden);
            }

            // Check that the requested enrolment exists
            var requestedEnrolment = await (
                from enrolment in _context.Enrolments
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where enrolment.EnrolmentID == result.EnrolmentID
                select new
                {
                    eventItem.OrganiserID
                }
            ).FirstOrDefaultAsync();

            if (requestedEnrolment == null)
            {
                return (false,
                    "The selected enrolment does not exist.",
                    StatusCodes.Status404NotFound);
            }

            // Prevent moving a result to another organiser's event
            if (requestedEnrolment.OrganiserID != organiserId)
            {
                return (false,
                    "You cannot use an enrolment from another organiser's event.",
                    StatusCodes.Status403Forbidden);
            }

            // Prevent duplicate results for the same enrolment
            bool duplicateResult = await _context.Results
                .AnyAsync(r =>
                    r.EnrolmentID == result.EnrolmentID
                    && r.ResultID != result.ResultID);

            if (duplicateResult)
            {
                return (false,
                    "A result already exists for this enrolment.",
                    StatusCodes.Status409Conflict);
            }

            if (result.FinishPosition <= 0)
            {
                return (false,
                    "Finish position must be greater than zero.",
                    StatusCodes.Status400BadRequest);
            }

            if (result.FinishTime <= TimeSpan.Zero)
            {
                return (false,
                    "Finish time must be greater than zero.",
                    StatusCodes.Status400BadRequest);
            }

            existingResult.EnrolmentID = result.EnrolmentID;
            existingResult.FinishPosition = result.FinishPosition;
            existingResult.FinishTime = result.FinishTime;

            await _context.SaveChangesAsync();

            return (true,
                "Result updated successfully.",
                StatusCodes.Status204NoContent);
        }
        // End of UpdateResult method

        // Beginning of GetPerformanceHistory method
        public async Task<List<Result>> GetPerformanceHistory(int userId)
        {
            return await (
                from result in _context.Results.AsNoTracking()
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                join eventItem in _context.Events
                    on enrolment.EventID equals eventItem.EventID
                where enrolment.UserID == userId
                orderby eventItem.EventDate descending,
                    result.ResultID descending
                select result
            ).ToListAsync();
        }
        // End of GetPerformanceHistory method
    }
    // End of ResultService class
}
// End of Result service namespace
