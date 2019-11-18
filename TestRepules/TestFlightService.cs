using Moq;
using Repules.Bll;
using Repules.Bll.Managers;
using Repules.Controllers;
using Repules.Dal;
using Repules.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace TestRepules
{
    public class TestFlightService
    {
        [Fact]
        public async void TestGetFlightAsync()
        {
            var id = Guid.NewGuid();
            var token = new CancellationToken();
            Flight flight = new Flight() { FlightId = id };
            var mock = new Mock<IFlightService>();
            mock.Setup(p => p.GetFlightAsync(flight.FlightId, token)).ReturnsAsync(flight);
            FlightManager flightManager = new FlightManager(mock.Object);
            var result = await flightManager.GetFlightAsync(flight.FlightId, token);
            Assert.Equal(result, flight);
        }

        [Fact]
        public async void TestGetFlightsAsync()
        {
            FlightStatus status = FlightStatus.Accepted;
            var token = new CancellationToken();
            List<Flight> flights = new List<Flight>() { new Flight() { FlightStatus = status } };
            var mock = new Mock<IFlightService>();
            mock.Setup(p => p.GetFlightsAsync(status, token)).ReturnsAsync(flights);
            FlightManager flightManager = new FlightManager(mock.Object);
            var result = await flightManager.GetFlightsAsync(status, token);
            Assert.Equal(result, flights);
        }
    }
}
