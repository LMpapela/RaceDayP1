
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayP1.Models;
using RaceDayP1.Services;
using System.Security.Claims;

// Beginning of Events controller namespace
namespace RaceDayP1.Controller
{
    // Beginning of EventsController class
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        // Beginning of EventsController constructor
        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }
        // End of EventsController constructor

        // Beginning of GetAllEvents method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEvents();

            return Ok(events);
        }
        // End of GetAllEvents method

        // Beginning of GetEvent method
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _eventService.GetEventById(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }
        // End of GetEvent method

        // Beginning of CreateEvent method
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(
            [FromBody] Event eventItem)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            // Assign the event to the authenticated organiser.
            eventItem.OrganiserID = currentUserId;

            try
            {
                var createdEvent =
                    await _eventService.CreateEvent(eventItem);

                return CreatedAtAction(
                    nameof(GetEvent),
                    new { id = createdEvent.EventID },
                    createdEvent);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // End of CreateEvent method

        // Beginning of UpdateEvent method
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            [FromBody] Event eventItem)
        {
            if (id != eventItem.EventID)
            {
                return BadRequest(new
                {
                    message = "The route ID must match the event ID."
                });
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var existingEvent = await _eventService.GetEventById(id);

            if (existingEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            if (existingEvent.OrganiserID != currentUserId)
            {
                return Forbid();
            }

            try
            {
                var updated = await _eventService.UpdateEvent(
                    eventItem,
                    currentUserId);

                if (!updated)
                {
                    return NotFound(new { message = "Event not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // End of UpdateEvent method

        // Beginning of DeleteEvent method
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized();
            }

            var existingEvent = await _eventService.GetEventById(id);

            if (existingEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            if (existingEvent.OrganiserID != currentUserId)
            {
                return Forbid();
            }

            try
            {
                var deleted = await _eventService.DeleteEvent(
                    id,
                    currentUserId);

                if (!deleted)
                {
                    return NotFound(new { message = "Event not found." });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // End of DeleteEvent method
    }
    // End of EventsController class
}
// End of Events controller namespace
