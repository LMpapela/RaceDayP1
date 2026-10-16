
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

            // Organisers can view all enrolments.
            if (User.IsInRole("Organiser"))
            {
                var allEnrolments =
                    await _enrolmentService.GetAllEnrolments();

                return Ok(allEnrolments);
            }

            // Participants can only view their own enrolments.
            if (User.IsInRole("Participant"))
            {
                var userEnrolments =
                    await _enrolmentService.GetEnrolmentsForUser(
                        currentUserId);

                return Ok(userEnrolments);
            }

            return Forbid();
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

            // Organisers can view any enrolment.
            if (User.IsInRole("Organiser"))
            {
                var organiserEnrolment =
                    await _enrolmentService.GetEnrolmentById(id);

                if (organiserEnrolment == null)
                {
                    return NotFound(new
                    {
                        message = "Enrolment not found."
                    });
                }

                return Ok(organiserEnrolment);
            }

            // Participants can only view their own enrolment.
            if (User.IsInRole("Participant"))
            {
                var participantEnrolment =
                    await _enrolmentService.GetEnrolmentByIdForUser(
                        id,
                        currentUserId);

                if (participantEnrolment == null)
                {
                    return NotFound(new
                    {
                        message = "Enrolment not found."
                    });
                }

                return Ok(participantEnrolment);
            }

            return Forbid();
        }
        // End of GetEnrolment method

        // Beginning of CreateEnrolment method
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<ActionResult<Enrolment>> CreateEnrolment(
            [FromBody] Enrolment enrolment)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            if (enrolment == null)
            {
                return BadRequest(new
                {
                    message = "Enrolment details are required."
                });
            }

            // Always use the authenticated participant's ID.
            enrolment.UserID = currentUserId;

            var result =
                await _enrolmentService.CreateEnrolment(enrolment);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
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

            // Verify that this enrolment belongs to the participant.
            var enrolment =
                await _enrolmentService.GetEnrolmentByIdForUser(
                    id,
                    currentUserId);

            if (enrolment == null)
            {
                return NotFound(new
                {
                    message = "Enrolment not found."
                });
            }

            try
            {
                var deleted =
                    await _enrolmentService.DeleteEnrolment(id);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        message = "Enrolment not found."
                    });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Preserve enrolments with recorded results.
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        // End of DeleteEnrolment method
    }
    // End of EnrolmentsController class
}
// End of Enrolments controller namespace
