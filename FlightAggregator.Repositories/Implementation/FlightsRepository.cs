using Dapper;
using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlightAggregator.Repositories.Implementation
{
    public class FlightsRepository : DataMapperBase<FlightsRepository>, IFlightsRepository
    {
        private string _connectionString = string.Empty;
        IConfiguration _configuration;
        public FlightsRepository(ILogger<FlightsRepository> logger, IConfiguration configuration) : base(logger, configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("flightDb") ?? "";
        }

        public async Task<bool> BookFlight(FlightBookDto dto)
        {
            var queryParams = new DynamicParameters();
            queryParams.Add("@FlightId", dto.FlightId);

            var query = $"UPDATE MockFlightsData SET IsAvailable = 0 WHERE Id = @FlightId";

            var updateResult = await OpenConnectionAndDoAction(_connectionString, async conn =>
            {
                return await conn.ExecuteAsync(query, queryParams);
            });

            return updateResult > 0 ? true : false;
        }

        public async Task<List<FlightDataModel>> GetFlights(FlightSearchDto dto, int companyId)
        {
            var returnData = new List<FlightDataModel>();

            var queryParams = new DynamicParameters();
            queryParams.Add("@From", dto.From);
            queryParams.Add("@To", dto.To);
            queryParams.Add("@FlightDate", dto.FlightDate.Date);
            queryParams.Add("@CompanyId", companyId);


            var query = @"SELECT * FROM MockFlightsData 
                            WHERE [From] = @From AND [To] = @To 
                            AND FlightDate = @FlightDate AND IsAvailable = 1 AND CompanyId = @CompanyId";

            returnData = await OpenConnectionAndDoAction(_connectionString, async conn =>
            {
                var res = await conn.QueryAsync<FlightDataModel>(query, queryParams);
                return new List<FlightDataModel>(res.ToList());
            });

            return returnData;
        }
    }
}

