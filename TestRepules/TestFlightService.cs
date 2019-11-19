using Microsoft.EntityFrameworkCore;
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
        //[Fact]
        //public async void TestGetFlightAsync()
        //{
        //    var id = Guid.NewGuid();
        //    var token = new CancellationToken();
        //    Flight flight = new Flight() { FlightId = id };

        //    var data = new List<Flight>
        //    {
        //        new Flight { FlightId = id }
        //    }.AsQueryable();

        //    var mock = new Mock<IFlightService>();
        //    mock.Setup(p => p.GetFlightAsync(flight.FlightId, token)).ReturnsAsync(flight);
        //    var mock2 = new Mock<IFlightLogFileService>();
        //    var mock3 = new Mock<IGPSRecordService>();

        //    var mockSet = new Mock<DbSet<Flight>>();
        //    mockSet.As<IQueryable<Flight>>().Setup(m => m.Provider).Returns(data.Provider);
        //    mockSet.As<IQueryable<Flight>>().Setup(m => m.Expression).Returns(data.Expression);
        //    mockSet.As<IQueryable<Flight>>().Setup(m => m.ElementType).Returns(data.ElementType);
        //    mockSet.As<IQueryable<Flight>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        //    var mockContext = new Mock<ApplicationContext>();
        //    mockContext.Setup(m => m.Flights).Returns(mockSet.Object);


        //    FlightManager flightManager = new FlightManager( mock.Object, mock2.Object, mockContext.Object,mock3.Object);
        //    var result = await flightManager.GetFlightAsync(flight.FlightId, token);
        //    Assert.Equal(result, flight);
        //}

        //[Fact]
        //public async void TestGetFlightsAsync()
        //{
        //    FlightStatus status = FlightStatus.Accepted;
        //    var token = new CancellationToken();
        //    List<Flight> flights = new List<Flight>() { new Flight() { FlightStatus = status } };
        //    var mock = new Mock<IFlightService>();
        //    mock.Setup(p => p.GetFlightsAsync(status, token)).ReturnsAsync(flights);
        //    FlightManager flightManager = new FlightManager(mock.Object);
        //    var result = await flightManager.GetFlightsAsync(status, token);
        //    Assert.Equal(result, flights);
        //}
    }
}
