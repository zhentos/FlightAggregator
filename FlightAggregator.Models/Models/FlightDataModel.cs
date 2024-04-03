namespace FlightAggregator.Models.Models
{
    public class FlightDataModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set;}
        public string From { get; set; }
        public string To { get; set; }
        public DateTime FlightDate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
