using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Repositories.Interfaces;
using FlightAggregator.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlightAggregator.Services.Implementations.FlightProviders
{
    public class AuroraFlightsService : IFlightProvidersService
    {
        private readonly IFlightsRepository _flightsRepository;
        private readonly ILogger<AuroraFlightsService> _logger;
        //hardcoded company id for test purposes, can be obtained from config in a real app.
        private const int _companyId = 3;

        public AuroraFlightsService(IFlightsRepository flightsRepository, ILogger<AuroraFlightsService> logger)
        {
            _flightsRepository = flightsRepository;
            _logger = logger;
        }
        public async Task<bool> BookFlight(FlightBookDto dto)
        {
            var result = false;
            try
            {
                return await _flightsRepository.BookFlight(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }

        public async Task<List<FlightDataModel>> GetFlights(FlightSearchDto dto)
        {
            try
            {
                return await _flightsRepository.GetFlights(dto, _companyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return [];
        }
    }
}
