using System;
using Repules.Dal;
using Repules.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using Hangfire;
using Microsoft.Extensions.Logging;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Repules.Bll
{
    internal class FlightService : IFlightService
    {
        private readonly ApplicationContext applicationContext;
        private readonly ILogger<FlightService> logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FlightService(ApplicationContext applicationContext, ILogger<FlightService> logger, IHttpContextAccessor _httpContextAccessor)
        {
            this.applicationContext = applicationContext;
            this.logger = logger;
            this._httpContextAccessor = _httpContextAccessor;
        }
        public async Task<List<Flight>> GetFlightsAsync(FlightStatus? flightStatus, CancellationToken cancellationToken)
        {
            IQueryable<Flight> query = applicationContext.Flights.Include(f => f.DepartureLocation).Include(f => f.ArrivalLocation).Include(f => f.ApplicationUser);
            if (flightStatus.HasValue)
            {
                query = query.Where(f => f.FlightStatus == flightStatus);
            }
            List<Flight> flightList = await query.ToListAsync(cancellationToken);
            return flightList;
        }
        public async Task<Flight> GetFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = applicationContext.Flights.Include(f => f.DepartureLocation).Include(f => f.ArrivalLocation).Include(f => f.GPSRecords).Include(f => f.ApplicationUser);
            return await query.SingleOrDefaultAsync(f => f.FlightId == id, cancellationToken);
        }

        public async Task<List<Flight>> GetUserFlightsAsync(CancellationToken cancellationToken)
        {
            Guid actUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); //actualUserId
            List<Flight> flightList = await applicationContext.Flights.Include(f => f.DepartureLocation).Include(f => f.ArrivalLocation).Where(f => f.ApplicationUserId == actUserId).ToListAsync(cancellationToken);
            return flightList;
        }

        public async Task<Flight> GetUserFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            Guid actUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); //actualUserId
            var query = applicationContext.Flights.Include(f => f.DepartureLocation).Include(f => f.ArrivalLocation).Include(f => f.GPSRecords).Where(f => f.ApplicationUserId == actUserId);
            return await query.SingleOrDefaultAsync(f => f.FlightId == id, cancellationToken);
        }

        public async Task AddFlightAsync(Flight flight, CancellationToken cancellationToken = default(CancellationToken))
        {
            await applicationContext.Flights.AddAsync(flight, cancellationToken);
        }

        public void UpdateFlight(Flight flight)
        {
            applicationContext.Flights.Update(flight);
        }

        public async Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            Flight flight = await applicationContext.Flights.SingleOrDefaultAsync(f => f.FlightId == id, cancellationToken);
            if (flight == null)
                return;
            applicationContext.Flights.Remove(flight);
        }

        public void ParseString(string line, Flight flight)
        {
            if (line.StartsWith("HFDTE"))
            {
                string datestr1 = line.Substring(5, 4);
                string datestr2 = line.Substring(9, 2);
                string datestr = datestr1 + "20" + datestr2;
                flight.Date = DateTime.ParseExact(datestr,
                      "ddMMyyyy",
                       CultureInfo.InvariantCulture);
            }
            else if (line.StartsWith("B"))
            {
                GPSRecord record = new GPSRecord();
                string datestring = line.Substring(1, 6);
                string latitudestring = line.Substring(7, 7);
                string longitudestring = line.Substring(15, 8);
                string altitudestring = line.Substring(25, 5);

                record.TimeStamp = DateTime.ParseExact(datestring,
                      "HHmmss",
                       CultureInfo.InvariantCulture);
                ////
                double deg = Convert.ToDouble(latitudestring.Substring(0, 2));
                double wholemin = Convert.ToDouble(latitudestring.Substring(2, 2));
                double fracmin = Convert.ToDouble(latitudestring.Substring(4, 3));
                double fraction = (wholemin + fracmin / 1000) / 60;
                double latitude = deg + fraction;
                record.Latitude = latitude;
                ////
                double deg2 = Convert.ToDouble(longitudestring.Substring(0, 3));
                double wholemin2 = Convert.ToDouble(longitudestring.Substring(3, 2));
                double fracmin2 = Convert.ToDouble(longitudestring.Substring(5, 3));
                var fraction2 = (wholemin2 + fracmin2 / 1000) / 60;
                var longitude = deg2 + fraction2;
                record.Longitude = longitude;
                record.Altitude = Convert.ToInt32(altitudestring); //méter
                record.Flight = flight;
                flight.GPSRecords.Add(record); //elmentem egy listába
            }
        }

        public async Task SetAirportsAsync(Flight flight, CancellationToken cancellationToken = default(CancellationToken))
        {
            var recmin = flight.GPSRecords.OrderBy(r => r.TimeStamp).First();
            var recmax = flight.GPSRecords.OrderBy(r => r.TimeStamp).Last();

            TimeSpan interval = recmax.TimeStamp - recmin.TimeStamp;
            (flight.DurationHours, flight.DurationMins, flight.DurationSeconds) = CalculateDuration(flight);
            //departure, arrival kiszamolasa
            var airports = await applicationContext.Airports.ToListAsync(cancellationToken);
            foreach (var airport in airports)
            {
                var distanceFromStart = CalculateDistance(recmin, airport);
                var distanceFromStop = CalculateDistance(recmax, airport);
                logger.LogInformation($"{airport.Longitude} {airport.Latitude} {recmin.Longitude} {recmin.Latitude} {distanceFromStart}");
                if (distanceFromStart < 3000) //departure
                {
                    flight.DepartureLocation = airport;
                    //break;
                }
                if (distanceFromStop < 3000)
                {
                    flight.ArrivalLocation = airport;
                    //break;
                }
                //ha nincs repter a maxrec-hez -> "terep"

            }
        }

        private double CalculateDistance(GPSRecord start, Airport end)
        {
            var sCoord = new GeoCoordinate(start.Latitude, start.Longitude);
            var eCoord = new GeoCoordinate(end.Latitude, end.Longitude);
            return sCoord.GetDistanceTo(eCoord); //meterben adja vissza
        }

        private double CalculateDistance(GPSRecord start, GPSRecord end)
        {
            var sCoord = new GeoCoordinate(start.Latitude, start.Longitude);
            var eCoord = new GeoCoordinate(end.Latitude, end.Longitude);
            return sCoord.GetDistanceTo(eCoord); //meterben adja vissza
        }

        //extra 1
        private (int, int, int) CalculateDuration(Flight flight)
        {
            var list = flight.GPSRecords.ToList();
            List<GPSRecord> morethan40 = new List<GPSRecord>();
            for (var i = 0; i < list.Count - 1; i++) //vegigmegyünk a gps rekordokon
            {
                var first = list[i];
                var second = list[i + 1];
                double dist = CalculateDistance(first, second); //elso, masodik rekord távolsága
                //köztük eltelt idő
                TimeSpan interval = second.TimeStamp - first.TimeStamp;
                double tim = interval.TotalSeconds;
                double refmeterpersec = 11.1111111;
                if (dist / tim > refmeterpersec)
                {
                    morethan40.Add(first);
                }
            }
            TimeSpan duration = morethan40.Last().TimeStamp - morethan40.First().TimeStamp;
            return (duration.Hours, duration.Minutes, duration.Seconds);
        }

    }
}
