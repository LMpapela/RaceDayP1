// Beginning of Category model namespace
namespace RaceDayP1.Models
{
    // Beginning of Category class
    public class Category
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
    // End of Category class
}
// End of Category model namespace