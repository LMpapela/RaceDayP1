using System.ComponentModel.DataAnnotations;

// Beginning of Event model namespace
namespace RaceDayP1.Models
{
    // Beginning of Event class
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Range(0.1, 1000)]
        public decimal Distance { get; set; }

        [Required]
        public int EventTypeID { get; set; }

        [Required]
        public int OrganiserID { get; set; }
    }
    // End of Event class
}
// End of Event model namespace