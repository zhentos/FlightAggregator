namespace FlightAggregator.Models.Dto_s
{
    public record FlightSearchDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime FlightDate { get; set; }
    }
}
