using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Repules.Model;
using Repules.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Repules.Bll.Managers;
using System.Threading;

namespace Repules.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminFlightController : Controller
    {
        public readonly FlightManager flightManager;

        public AdminFlightController(FlightManager flightManager)
        {
            this.flightManager = flightManager;
        }
        public async Task<IActionResult> Index(FlightStatus? flightStatus, CancellationToken cancellationToken)
        {
            List<Flight> flights = await flightManager.GetFlightsAsync(flightStatus, cancellationToken);
            List<AdminFlightViewModel> adminFlightViewModels = new List<AdminFlightViewModel>();
            foreach (var f in flights)
            {
                AdminFlightViewModel adminFlightViewModel = new AdminFlightViewModel();
                adminFlightViewModel.UserName = f.ApplicationUser.UserName;
                adminFlightViewModel.FlightId = f.FlightId;
                adminFlightViewModel.Date = f.Date.ToString("yyyy-MM-dd");
                adminFlightViewModel.Duration = String.Format("{0} óra, {1} perc, {2} másodperc", f.DurationHours, f.DurationMins, f.DurationSeconds);
                adminFlightViewModel.DepartureName = f.DepartureLocation.Name;
                adminFlightViewModel.ArrivalName = f.ArrivalLocation?.Name;
                if (adminFlightViewModel.ArrivalName == null)
                {
                    adminFlightViewModel.ArrivalName = "Terep";
                }
                adminFlightViewModel.Status = f.FlightStatus.GetDescription();
                adminFlightViewModels.Add(adminFlightViewModel);

            }
            return View(adminFlightViewModels);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            Flight flight = await flightManager.GetFlightAsync(id, cancellationToken);
            AdminFlightViewModel adminFlightViewModel = new AdminFlightViewModel()
            {

                UserName = flight.ApplicationUser.UserName,
                FlightId = flight.FlightId,
                Date = flight.Date.ToString("yyyy-MM-dd"),
                Duration = String.Format("{0} óra, {1} perc, {2} másodperc", flight.DurationHours, flight.DurationMins, flight.DurationSeconds),
                DepartureName = flight.DepartureLocation.Name,
                ArrivalName = flight.ArrivalLocation?.Name,
                Status = flight.FlightStatus.GetDescription(),
                OptGPSRecords = JsonConvert.SerializeObject(flight.OptimizedGPSRecords.Select(x => new
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
            if (adminFlightViewModel.ArrivalName == null)
            {
                adminFlightViewModel.ArrivalName = "Terep";
            }
            return View(adminFlightViewModel);
        }

        public async Task<IActionResult> Accept(Guid FlightId, CancellationToken cancellationToken)
        {
            Flight flight = await flightManager.GetFlightAsync(FlightId, cancellationToken);
            if (flight.FlightStatus != FlightStatus.WaitingForAcceptance)
            {
                return BadRequest();
            }
            else
            {
                flight.FlightStatus = FlightStatus.Accepted;
                await flightManager.UpdateFlightAsync(flight, cancellationToken);

                return RedirectToAction("Index", "AdminFlight");
            }
        }

        public async Task<IActionResult> Decline(Guid FlightId, CancellationToken cancellationToken)
        {
            Flight flight = await flightManager.GetFlightAsync(FlightId, cancellationToken);
            if (flight.FlightStatus != FlightStatus.WaitingForAcceptance)
            {
                return BadRequest();
            }
            else
            {
                flight.FlightStatus = FlightStatus.Declined;
                await flightManager.UpdateFlightAsync(flight, cancellationToken);

                return RedirectToAction("Index", "AdminFlight");
            }
        }

    }
}
