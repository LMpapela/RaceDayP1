using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Enrolments controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of EnrolmentsController class
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        // Beginning of EnrolmentsController constructor
        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of EnrolmentsController constructor

        // Beginning of GetEnrolments method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
        {
            var enrolments = await _context.Enrolments.ToListAsync();

            return Ok(enrolments);
        }
        // End of GetEnrolments method

        // Beginning of GetEnrolment method
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
            {
                return NotFound();
            }

            return Ok(enrolment);
        }
        // End of GetEnrolment method

        // Beginning of CreateEnrolment method
        [HttpPost]
        public async Task<ActionResult<Enrolment>> CreateEnrolment(
            Enrolment enrolment)
        {
            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = enrolment.EnrolmentID },
                enrolment);
        }
        // End of CreateEnrolment method

        // Beginning of DeleteEnrolment method
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
            {
                return NotFound();
            }

            _context.Enrolments.Remove(enrolment);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // End of DeleteEnrolment method
    }
    // End of EnrolmentsController class
}
// End of Enrolments controller namespace