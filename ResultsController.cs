
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Security.Claims;

// Beginning of Results controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of ResultsController class
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResultsController : ControllerBase
    {
        private readonly ResultService _resultService;

        // Beginning of ResultsController constructor
        public ResultsController(ResultService resultService)
        {
            _resultService = resultService;
        }
        // End of ResultsController constructor

        // Beginning of GetResults method
        [HttpGet]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var results =
                await _resultService.GetResultsForOrganiser(organiserId);

            return Ok(results);
        }
        // End of GetResults method

        // Beginning of GetResult method
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            if (User.IsInRole("Organiser"))
            {
                var organiserResult =
                    await _resultService.GetResultByIdForOrganiser(
                        id,
                        currentUserId);

                if (organiserResult == null)
                {
                    return NotFound(
                        "Result not found or you do not organise its event.");
                }

                return Ok(organiserResult);
            }

            if (User.IsInRole("Participant"))
            {
                var participantResult =
                    await _resultService.GetResultByIdForUser(
                        id,
                        currentUserId);

                if (participantResult == null)
                {
                    return NotFound();
                }

                return Ok(participantResult);
            }

            return Forbid();
        }
        // End of GetResult method

        // Beginning of CreateResult method
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateResult(Result result)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var creationResult =
                await _resultService.CreateResult(result, organiserId);

            if (!creationResult.Success)
            {
                return StatusCode(
                    creationResult.StatusCode,
                    creationResult.Message);
            }

            return CreatedAtAction(
                nameof(GetResult),
                new { id = creationResult.Result!.ResultID },
                creationResult.Result);
        }
        // End of CreateResult method

        // Beginning of UpdateResult method
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(
            int id,
            Result result)
        {
            if (id != result.ResultID)
            {
                return BadRequest(
                    "The route ID must match the result ID.");
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var updateResult =
                await _resultService.UpdateResult(result, organiserId);

            if (!updateResult.Success)
            {
                return StatusCode(
                    updateResult.StatusCode,
                    updateResult.Message);
            }

            return NoContent();
        }
        // End of UpdateResult method

        // Beginning of GetPerformanceHistory method
        [HttpGet("my/history")]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<IEnumerable<Result>>>
            GetPerformanceHistory()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var results =
                await _resultService.GetPerformanceHistory(currentUserId);

            return Ok(results);
        }
        // End of GetPerformanceHistory method
    }
    // End of ResultsController class
}
// End of Results controller namespace
