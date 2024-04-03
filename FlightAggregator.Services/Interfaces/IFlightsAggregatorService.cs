using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Services.Infrastructure;

namespace FlightAggregator.Services.Interfaces
{
    public interface IFlightsAggregatorService
    {
        Task<ValidationResult<List<FlightDataModel>>> SearchFlights(FlightSearchDto dto);
        Task<ValidationResult<bool>> BookFlight(FlightBookDto dto);
    }
}
