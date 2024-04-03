using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace FlightAggregator.Repositories
{
    public abstract class DataMapperBase<T> where T : class
    {
        protected readonly ILogger<T> _logger;
        private readonly IConfiguration _configuration;

        protected DataMapperBase(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected async Task<TResult> OpenConnectionAndDoAction<TResult>(
          string connectionString,
          Func<SqlConnection, Task<TResult>> payload)
        {
            await using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                        await connection.OpenAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                }

                return await payload(connection);
            }
        }
    }
}
