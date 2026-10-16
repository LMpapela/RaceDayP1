
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
            return await _context.Events
                .AsNoTracking()
                .ToListAsync();
        }
        // End of GetAllEvents method

        // Beginning of GetEventById method
        public async Task<Event?> GetEventById(int id)
        {
            return await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);
        }
        // End of GetEventById method

        // Beginning of ValidateEvent method
        private async Task ValidateEvent(Event eventItem)
        {
            if (string.IsNullOrWhiteSpace(eventItem.EventName))
            {
                throw new InvalidOperationException(
                    "Event name is required.");
            }

            if (string.IsNullOrWhiteSpace(eventItem.Location))
            {
                throw new InvalidOperationException(
                    "Event location is required.");
            }

            if (eventItem.Distance < 0.1m ||
                eventItem.Distance > 1000m)
            {
                throw new InvalidOperationException(
                    "Event distance must be between 0.1 and 1000.");
            }

            if (eventItem.EventDate == default)
            {
                throw new InvalidOperationException(
                    "A valid event date is required.");
            }

            bool eventTypeExists = await _context.EventTypes
                .AnyAsync(t => t.EventTypeID == eventItem.EventTypeID);

            if (!eventTypeExists)
            {
                throw new InvalidOperationException(
                    "The selected event type does not exist.");
            }

            // Remove unnecessary spaces from text fields.
            eventItem.EventName = eventItem.EventName.Trim();
            eventItem.Location = eventItem.Location.Trim();

            if (eventItem.Description != null)
            {
                eventItem.Description = eventItem.Description.Trim();
            }
        }
        // End of ValidateEvent method

        // Beginning of CreateEvent method
        public async Task<Event> CreateEvent(Event eventItem)
        {
            await ValidateEvent(eventItem);

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
            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventItem.EventID);

            if (existingEvent == null)
            {
                return false;
            }

            // Only the organiser who owns the event can update it.
            if (existingEvent.OrganiserID != organiserId)
            {
                return false;
            }

            await ValidateEvent(eventItem);

            existingEvent.EventName = eventItem.EventName;
            existingEvent.Description = eventItem.Description;
            existingEvent.EventDate = eventItem.EventDate;
            existingEvent.Location = eventItem.Location;
            existingEvent.Distance = eventItem.Distance;
            existingEvent.EventTypeID = eventItem.EventTypeID;

            // Ownership cannot be transferred through an update.
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
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return false;
            }

            // Only the organiser who owns the event can delete it.
            if (eventItem.OrganiserID != organiserId)
            {
                return false;
            }

            // Preserve events that have participant enrolments.
            bool hasEnrolments = await _context.Enrolments
                .AnyAsync(e => e.EventID == id);

            if (hasEnrolments)
            {
                throw new InvalidOperationException(
                    "This event cannot be deleted because it has enrolments.");
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
