using Microsoft.EntityFrameworkCore;
using RaceDayP1.Data;
using RaceDayP1.Models;

// Beginning of Event service namespace
namespace RaceDayP1.Services
{
    // Beginning of EventService class
    public class EventService
    {
        private readonly RaceDayDbContext _context;

        // Beginning of EventService constructor
        public EventService(RaceDayDbContext context)
        {
            _context = context;
        }
        // End of EventService constructor

        // Beginning of GetAllEvents method
        public async Task<List<Event>> GetAllEvents()
        {
            return await _context.Events.ToListAsync();
        }
        // End of GetAllEvents method

        // Beginning of GetEventById method
        public async Task<Event?> GetEventById(int id)
        {
            return await _context.Events.FindAsync(id);
        }
        // End of GetEventById method

        // Beginning of CreateEvent method
        public async Task<Event> CreateEvent(Event eventItem)
        {
            _context.Events.Add(eventItem);

            await _context.SaveChangesAsync();

            return eventItem;
        }
        // End of CreateEvent method

        // Beginning of UpdateEvent method
        public async Task<bool> UpdateEvent(
            Event eventItem,
            int organiserId)
        {
            var existingEvent =
                await _context.Events.FindAsync(eventItem.EventID);

            if (existingEvent == null)
            {
                return false;
            }

            // Check that the organiser owns the event
            if (existingEvent.OrganiserID != organiserId)
            {
                return false;
            }

            existingEvent.EventName = eventItem.EventName;
            existingEvent.Description = eventItem.Description;
            existingEvent.EventDate = eventItem.EventDate;
            existingEvent.Location = eventItem.Location;
            existingEvent.Distance = eventItem.Distance;
            existingEvent.EventTypeID = eventItem.EventTypeID;

            // Keep the original owner
            existingEvent.OrganiserID = organiserId;

            await _context.SaveChangesAsync();

            return true;
        }
        // End of UpdateEvent method

        // Beginning of DeleteEvent method
        public async Task<bool> DeleteEvent(
            int id,
            int organiserId)
        {
            var eventItem =
                await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return false;
            }

            // Check that the organiser owns the event
            if (eventItem.OrganiserID != organiserId)
            {
                return false;
            }

            _context.Events.Remove(eventItem);

            await _context.SaveChangesAsync();

            return true;
        }
        // End of DeleteEvent method
    }
    // End of EventService class
}
// End of Event service namespace