namespace FlightAggregator.Models.Models
{
    public class InMemoryCacheModel
    {
        public int SlidingExpiration { get; set; }
        public int AbsoluteExpiration { get; set; }
    }
}
