// Beginning of Result model namespace
namespace RaceDayP1.Models
{
    // Beginning of Result class
    public class Result
    {
        public int ResultID { get; set; }

        public int EnrolmentID { get; set; }

        public int FinishPosition { get; set; }

        public TimeSpan FinishTime { get; set; }
    }
    // End of Result class
}
// End of Result model namespace