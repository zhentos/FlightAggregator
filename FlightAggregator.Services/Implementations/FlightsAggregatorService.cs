using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Repositories.Interfaces;
using FlightAggregator.Services.Infrastructure;
using FlightAggregator.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightAggregator.Services.Implementations
{
    public class FlightsAggregatorService : IFlightsAggregatorService
    {
        private readonly IEnumerable<IFlightProvidersService> _providers;
        private readonly IFlightsRepository _flightsRepository;
        private readonly ILogger<FlightsAggregatorService> _logger;
        private IMemoryCache _cache;
        private InMemoryCacheModel _inMemoryCacheModel { get; }
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public FlightsAggregatorService(
            IEnumerable<IFlightProvidersService> providers,
            IFlightsRepository flightsRepository,
            ILogger<FlightsAggregatorService> logger,
            IMemoryCache cache,
            IOptions<InMemoryCacheModel> options)
        {
            _providers = providers;
            _flightsRepository = flightsRepository;
            _logger = logger;
            _cache = cache;
            _inMemoryCacheModel = options.Value;
        }

        public async Task<ValidationResult<bool>> BookFlight(FlightBookDto dto)
        {
            if (dto == null) { return new ValidationResult<bool>(false); }

            var result = await _flightsRepository.BookFlight(dto);

            return result == false ? new ValidationResult<bool>("An error has occured!", false)
                                   : new ValidationResult<bool>(result);

        }

        public async Task<ValidationResult<List<FlightDataModel>>> SearchFlights(FlightSearchDto dto)
        {
            if (dto == null)
            {
                _logger.LogError("Bad input data was provided...here we can add user info as well as request data.");

                return new
                    ValidationResult<List<FlightDataModel>>("Flight search data can't be empty!", []);
            }
            var flightsRange = new List<FlightDataModel>();

            var allFlights = new ValidationResult<List<FlightDataModel>>([]);

            var cacheKey = $"{dto.From.ToLower()}_{dto.To.ToLower()}_{dto.FlightDate.Date.ToShortDateString()}";

            try
            {
                await semaphore.WaitAsync();

                if (_cache.TryGetValue(cacheKey, out flightsRange))
                {
                    allFlights.Value.AddRange(flightsRange ?? []);
                    return allFlights;
                }
                else
                {
                    await foreach (var (statusCode, flights) in EmulateRemoteServicesHandling(dto))
                    {
                        allFlights.Value.AddRange(flights);
                    }

                    if (allFlights.Value != null && allFlights.Value.Any())
                    {
                        var cacheOptions = GetCacheOptions();
                        AddToCache(cacheKey, allFlights.Value, cacheOptions);
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }

            return allFlights;
        }

        //В данном методе я просто эмулирую логику вызова и обработки ответов от различных провайдеров авиабилетов
        private async IAsyncEnumerable<(string statusCode, List<FlightDataModel> flights)>
            EmulateRemoteServicesHandling(FlightSearchDto dto)
        {
            foreach (var provider in _providers)
            {
                var flights = await provider.GetFlights(dto);
                //здесь я просто захардкодил 200 код, эмулируя ответ от реального сервиса
                yield return ("200", flights);
            }
        }
        private MemoryCacheEntryOptions GetCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(_inMemoryCacheModel.SlidingExpiration))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_inMemoryCacheModel.AbsoluteExpiration));
        }
        private void AddToCache(string key, object objectToAdd, MemoryCacheEntryOptions cacheEntryOptions)
        {
            try
            {
                _cache.Set(key, objectToAdd, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, objectToAdd, "Can't push the value into IMemoryCache.");
            }
        }
    }
}
