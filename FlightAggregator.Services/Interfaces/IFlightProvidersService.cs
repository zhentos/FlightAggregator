using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;

namespace FlightAggregator.Services.Interfaces
{
    public interface IFlightProvidersService
    {
        Task<List<FlightDataModel>> GetFlights(FlightSearchDto dto);
        Task<bool> BookFlight(FlightBookDto dto);
    }
}
