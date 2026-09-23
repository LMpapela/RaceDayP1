using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Security.Claims;

// Beginning of Enrolments controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of EnrolmentsController class
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrolmentsController : ControllerBase
    {
        private readonly EnrolmentService _enrolmentService;

        // Beginning of EnrolmentsController constructor
        public EnrolmentsController(EnrolmentService enrolmentService)
        {
            _enrolmentService = enrolmentService;
        }
        // End of EnrolmentsController constructor

        // Beginning of GetEnrolments method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            // Organisers can view all participant enrolments
            if (User.IsInRole("Organiser"))
            {
                var allEnrolments =
                    await _enrolmentService.GetAllEnrolments();

                return Ok(allEnrolments);
            }

            // Participants can only view their own enrolments
            var userEnrolments =
                await _enrolmentService.GetEnrolmentsForUser(
                    currentUserId);

            return Ok(userEnrolments);
        }
        // End of GetEnrolments method

        // Beginning of GetEnrolment method
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            // Organisers can view any enrolment
            if (User.IsInRole("Organiser"))
            {
                var organiserEnrolment =
                    await _enrolmentService.GetEnrolmentById(id);

                if (organiserEnrolment == null)
                {
                    return NotFound();
                }

                return Ok(organiserEnrolment);
            }

            // Participants can only view their own enrolment
            var participantEnrolment =
                await _enrolmentService.GetEnrolmentByIdForUser(
                    id,
                    currentUserId);

            if (participantEnrolment == null)
            {
                return NotFound();
            }

            return Ok(participantEnrolment);
        }
        // End of GetEnrolment method

        // Beginning of CreateEnrolment method
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<ActionResult<Enrolment>> CreateEnrolment(
            Enrolment enrolment)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            // Always use the authenticated user's ID
            enrolment.UserID = currentUserId;

            var result =
                await _enrolmentService.CreateEnrolment(enrolment);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = result.Enrolment!.EnrolmentID },
                result.Enrolment);
        }
        // End of CreateEnrolment method

        // Beginning of DeleteEnrolment method
        [Authorize(Roles = "Participant")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            // Check that the enrolment belongs to the participant
            var enrolment =
                await _enrolmentService.GetEnrolmentByIdForUser(
                    id,
                    currentUserId);

            if (enrolment == null)
            {
                return NotFound();
            }

            var deleted =
                await _enrolmentService.DeleteEnrolment(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        // End of DeleteEnrolment method
    }
    // End of EnrolmentsController class
}
// End of Enrolments controller namespace