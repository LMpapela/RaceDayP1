// Beginning of Event model namespace
namespace RaceDayP1.Models
{
    // Beginning of Event class
    public class Event
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public int EventTypeID { get; set; }

        public int OrganiserID { get; set; }
    }
    // End of Event class
}
// End of Event model namespace