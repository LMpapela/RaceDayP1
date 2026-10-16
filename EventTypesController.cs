
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of EventTypes controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of EventTypesController class
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class EventTypesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        // Beginning of EventTypesController constructor
        public EventTypesController(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of EventTypesController constructor

        // Beginning of GetEventTypes method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventType>>> GetEventTypes()
        {
            var eventTypes = await _context.EventTypes
                .AsNoTracking()
                .ToListAsync();

            return Ok(eventTypes);
        }
        // End of GetEventTypes method

        // Beginning of GetEventType method
        [HttpGet("{id}")]
        public async Task<ActionResult<EventType>> GetEventType(int id)
        {
            var eventType = await _context.EventTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventTypeID == id);

            if (eventType == null)
            {
                return NotFound();
            }

            return Ok(eventType);
        }
        // End of GetEventType method
    }
    // End of EventTypesController class
}
// End of EventTypes controller namespace
