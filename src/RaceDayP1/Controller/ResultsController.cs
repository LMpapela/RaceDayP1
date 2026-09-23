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
            var results = await _resultService.GetAllResults();

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
                    await _resultService.GetResultById(id);

                if (organiserResult == null)
                {
                    return NotFound();
                }

                return Ok(organiserResult);
            }

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
        // End of GetResult method

        // Beginning of CreateResult method
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateResult(Result result)
        {
            var creationResult =
                await _resultService.CreateResult(result);

            if (!creationResult.Success)
            {
                return BadRequest(creationResult.Message);
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
                return BadRequest();
            }

            var updateResult =
                await _resultService.UpdateResult(result);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }

            return NoContent();
        }
        // End of UpdateResult method

        // Beginning of GetPerformanceHistory method
        [HttpGet("my/history")]
        [Authorize(Roles = "Participant")]
        public async Task<ActionResult<IEnumerable<Result>>> GetPerformanceHistory()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var results =
                await _resultService.GetPerformanceHistory(
                    currentUserId);

            return Ok(results);
        }
        // End of GetPerformanceHistory method
    }
    // End of ResultsController class
}
// End of Results controller namespace