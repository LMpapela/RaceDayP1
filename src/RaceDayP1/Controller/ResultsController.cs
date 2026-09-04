using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Results controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of ResultsController class
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        // Beginning of ResultsController constructor
        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of ResultsController constructor

        // Beginning of GetResults method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            var results = await _context.Results.ToListAsync();

            return Ok(results);
        }
        // End of GetResults method

        // Beginning of GetResult method
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        // End of GetResult method

        // Beginning of CreateResult method
        [HttpPost]
        public async Task<ActionResult<Result>> CreateResult(Result result)
        {
            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }
        // End of CreateResult method

        // Beginning of UpdateResult method
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, Result result)
        {
            if (id != result.ResultID)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // End of UpdateResult method
    }
    // End of ResultsController class
}
// End of Results controller namespace