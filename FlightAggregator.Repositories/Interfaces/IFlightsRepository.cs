using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;

namespace FlightAggregator.Repositories.Interfaces
{
    public interface IFlightsRepository
    {
        Task<List<FlightDataModel>> GetFlights(FlightSearchDto dto, int companyId);
        Task<bool> BookFlight(FlightBookDto dto);
    }
}
