using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlightAggregator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightsAggregatorService _aggregatorService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightsAggregatorService aggregatorService, ILogger<FlightController> logger)
        {
            _aggregatorService = aggregatorService;
            _logger = logger;
        }
        /// <summary>
        /// Search new flights by input parameter
        /// </summary>
        /// <param name="searchDto">Parameters to search: Departure and arrival points and desired departure date</param>
        /// <response code="200">Returned search results according to the criteria.</response>
        /// <response code="400">Invalid search parameters.</response>
        [SwaggerResponse(StatusCodes.Status200OK, "Returned search results according to the criteria.", typeof(List<FlightDataModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid search parameters.")]
        [HttpPost("search")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightSearchDto searchDto)
        {
            var result = await _aggregatorService.SearchFlights(searchDto);

            _logger.LogInformation(result.ToString());

            return result.Success ? Ok(result.Value) : BadRequest(result.Messages);
        }

        /// <summary>
        /// Reserves the selected flight 
        /// </summary>
        /// <param name="searchDto">Parameters to search: Departure and arrival points and desired departure date</param>
        /// <response code="200">Returned booking confirmation result..</response>
        /// <response code="400">There's been an error. The flight was not booked.</response>
        [SwaggerResponse(StatusCodes.Status200OK, "Returned booking confirmation result.", typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "There's been an error. The flight was not booked.")]
        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] FlightBookDto bookDto)
        {
            var result = await _aggregatorService.BookFlight(bookDto);

            _logger.LogInformation(result.ToString());

            return result.Success? Ok(result.Value) : BadRequest(result.Messages);
        }
    }
}
