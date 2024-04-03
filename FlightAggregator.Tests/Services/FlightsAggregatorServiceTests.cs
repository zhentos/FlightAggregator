using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Repositories.Interfaces;
using FlightAggregator.Services.Implementations;
using FlightAggregator.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FlightAggregator.Tests.Services
{
    [TestClass]
    public class FlightsAggregatorServiceTests
    {
        private FlightsAggregatorService _flightsAggregatorService;
        private Mock<IFlightsRepository> _flightsRepository;  

        [TestInitialize]
        public void TestInitialize()
        {
            _flightsRepository = new Mock<IFlightsRepository>();

            _flightsAggregatorService = new FlightsAggregatorService(
                Mock.Of<IEnumerable<IFlightProvidersService>>(),
                _flightsRepository.Object,
                Mock.Of<ILogger<FlightsAggregatorService>>(),
                Mock.Of<IMemoryCache>(),
                Mock.Of<IOptions<InMemoryCacheModel>>());
        }

        [TestMethod]
        public async Task BookFlight_InvalidInputData_ReturnsErrorMessage()
        {
            //Arrange
            var dto = new FlightBookDto();
            _flightsRepository.Setup(s => s.BookFlight(It.IsAny<FlightBookDto>()))
                              .ReturnsAsync(false);

            //Act
            var result = await _flightsAggregatorService.BookFlight(dto);

            //Assert
            Assert.AreEqual(false, result.Value);
            Assert.AreEqual(1, result.Messages.Count());
        }
    }
}
