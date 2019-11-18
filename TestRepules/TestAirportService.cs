using Moq;
using Repules.Bll;
using Repules.Bll.Managers;
using Repules.Controllers;
using Repules.Dal;
using Repules.Model;
using System;
using System.Linq;
using Xunit;

namespace TestRepules
{
    public class TestAirportService
    {
        [Fact]
        public void TestGetAirport()
        {
            var id = Guid.NewGuid();
            Airport airport = new Airport() { AirportId = id };
            var mock = new Mock<IAirportService>();
            mock.Setup(p => p.GetAirport(airport.AirportId)).Returns(airport);
            AirportManager airportManager = new AirportManager(mock.Object);
            var result = airportManager.GetAirport(airport.AirportId);
            Assert.Equal(result, airport);
        }
    }
}
