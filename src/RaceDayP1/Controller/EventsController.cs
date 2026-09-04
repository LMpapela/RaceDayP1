using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Events controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of EventsController class
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        // Beginning of EventsController constructor
        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of EventsController constructor

        // Beginning of GetEvents method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _context.Events.ToListAsync();

            return Ok(events);
        }
        // End of GetEvents method

        // Beginning of GetEvent method
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }
        // End of GetEvent method

        // Beginning of CreateEvent method
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event eventItem)
        {
            _context.Events.Add(eventItem);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventID },
                eventItem);
        }
        // End of CreateEvent method

        // Beginning of UpdateEvent method
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventID)
            {
                return BadRequest();
            }

            _context.Entry(eventItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // End of UpdateEvent method

        // Beginning of DeleteEvent method
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // End of DeleteEvent method
    }
    // End of EventsController class
}
// End of Events controller namespace