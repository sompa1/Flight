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
using Xunit;

namespace TestRepules
{
    public class TestAirportService
    {
        //[Fact]
        //public void TestGetAirport()
        //{
        //    var id = Guid.NewGuid();
        //    Airport airport = new Airport() { AirportId = id };

        //    var data = new List<Airport>
        //    {
        //        new Airport { AirportId = id }
        //    }.AsQueryable();

        //    var mock = new Mock<IAirportService>();
        //    mock.Setup(p => p.GetAirport(airport.AirportId)).Returns(airport);

        //    var mockSet = new Mock<DbSet<Airport>>();
        //    var mockContext = new Mock<ApplicationContext>();
        //    mockContext.Setup(m => m.Airports).Returns(mockSet.Object);


        //    AirportManager airportManager = new AirportManager(mockContext.Object, mock.Object);
        //    var result = airportManager.GetAirport(airport.AirportId);
        //    Assert.Equal(result, airport);
        //}
    }
}
