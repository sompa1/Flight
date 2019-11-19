using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repules.Bll.Managers;
using Repules.Model;
using Repules.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Threading;

namespace Repules.Controllers
{
    [Authorize(Roles = "user")]
    public class UserFlightController : Controller
    {

        public readonly FlightManager flightManager;

        public UserFlightController(FlightManager flightManager)
        {
            this.flightManager = flightManager;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Flight> flights = await flightManager.GetUserFlightsAsync(cancellationToken);
            List<UserFlightViewModel> userFlightViewModels = new List<UserFlightViewModel>();
            foreach (var f in flights)
            {
                UserFlightViewModel userFlightViewModel = new UserFlightViewModel();
                userFlightViewModel.FlightId = f.FlightId;
                userFlightViewModel.Date = f.Date.ToString("yyyy-MM-dd");
                userFlightViewModel.Duration = String.Format("{0} óra, {1} perc, {2} másodperc", f.DurationHours, f.DurationMins, f.DurationSeconds);
                userFlightViewModel.DepartureName = f.DepartureLocation.Name;
                userFlightViewModel.ArrivalName = f.ArrivalLocation?.Name;
                if (userFlightViewModel.ArrivalName == null)
                {
                    userFlightViewModel.ArrivalName = "Terep";
                }
                userFlightViewModel.Status = f.FlightStatus.GetDescription();
                userFlightViewModels.Add(userFlightViewModel);

            }
            return View(userFlightViewModels);
        }

        [Authorize(Roles = "user")]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            Flight flight = await flightManager.GetUserFlightAsync(id, cancellationToken);
            UserFlightViewModel userFlightViewModel = new UserFlightViewModel()
            {
                FlightId = flight.FlightId,
                Date = flight.Date.ToString("yyyy-MM-dd"),
                Duration = String.Format("{0} óra, {1} perc, {2} másodperc", flight.DurationHours, flight.DurationMins, flight.DurationSeconds),
                DepartureName = flight.DepartureLocation.Name,
                ArrivalName = flight.ArrivalLocation?.Name,
                Status = flight.FlightStatus.GetDescription(),
                OptGPSRecords = JsonConvert.SerializeObject(flight.GPSRecords.Where(ent=> ent.IsOptimized).Select(x => new
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    ColorA = x.ColorA,
                    ColorR = x.ColorR,
                    ColorG = x.ColorG,
                    ColorB = x.ColorB
                })),
                GPSRecords = JsonConvert.SerializeObject(flight.GPSRecords.Select(x => new
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    ColorA = x.ColorA,
                    ColorR = x.ColorR,
                    ColorG = x.ColorG,
                    ColorB = x.ColorB
                }))
            };
            if (userFlightViewModel.ArrivalName == null)
            {
                userFlightViewModel.ArrivalName = "Terep";
            }
            return View(userFlightViewModel);
        }
    }
}