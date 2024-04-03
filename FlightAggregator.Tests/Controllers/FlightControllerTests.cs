using FlightAggregator.API.Controllers;
using FlightAggregator.Models.Dto_s;
using FlightAggregator.Models.Models;
using FlightAggregator.Services.Infrastructure;
using FlightAggregator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlightAggregator.Tests.Controllers
{
    [TestClass]
    public class FlightControllerTests
    {
        private FlightController _controller;
        private Mock<IFlightsAggregatorService> _mockFlightsAggregatorService;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockFlightsAggregatorService = new Mock<IFlightsAggregatorService>();

            _controller = new FlightController(_mockFlightsAggregatorService.Object,
                                                Mock.Of<ILogger<FlightController>>());
        }

        [TestMethod]
        public async Task SearchFlights_ReturnsOk_WithSearchData()
        {
            //Arrange
            var flights = new ValidationResult<List<FlightDataModel>>([]);


            var searchDto = new FlightSearchDto()
            { From = "Minsk", To = "Moskva", FlightDate = DateTime.Now.Date };

            _mockFlightsAggregatorService
                .Setup(s => s.SearchFlights(It.IsAny<FlightSearchDto>()))
                .ReturnsAsync(flights);

            //Act
            var result = await _controller.SearchFlights(searchDto);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as List<FlightSearchDto>;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task BookFlight_ReturnsBadRequestResult_WhenBookingWasNotSuccessed()
        {
            //Arrange
            var bookingData = new FlightBookDto() { CompanyId = 1, FlightId = 1 };
            var bookingResult = new ValidationResult<bool>("error", false);

            _mockFlightsAggregatorService
                .Setup(s => s.BookFlight(It.IsAny<FlightBookDto>()))
                .ReturnsAsync(bookingResult);

            //Act
            var result = await _controller.BookFlight(bookingData);

            //Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
        }
    }
}
