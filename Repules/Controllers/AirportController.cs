﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repules.Bll.Managers;
using Repules.Bll;
using Repules.Model;
using Repules.Models;
using Repules.Model.WeatherMapModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Repules.Controllers
{
    [Authorize(Roles = "admin")]
    public class AirportController : Controller
    {
        private readonly AirportManager airportManager;
        private readonly IWeatherService weatherService;
        private readonly IHostingEnvironment env;

        public AirportController(AirportManager airportManager, IWeatherService weatherService, IHostingEnvironment env)
        {
            this.airportManager = airportManager;
            this.weatherService = weatherService;
            this.env = env;
        }

        public IActionResult Index()
        {
            List<Airport> airports = airportManager.GetAirports();

            List <AirportViewModel> airportViewModels = airports.Select(a => new AirportViewModel()
            {
                AirportId = a.AirportId,
                Name = a.Name,
                WeatherIcon = weatherService.GetWeatherByCoords(a.Latitude, a.Longitude).Weather[0].Icon,
                WeatherMain = weatherService.GetWeatherByCoords(a.Latitude, a.Longitude).Weather[0].Description,
                Latitude = a.Latitude,
                Longitude = a.Longitude
            }).ToList();

            return View(airportViewModels);
        }

        //GET : /AIRPORT/CREATE
        public IActionResult Create()
        {
            return View(); //itt kell feltölteni az excel fájlt
        }

        [HttpPost]
        public async Task<IActionResult> Create(AirportViewModel airportViewModel, CancellationToken cancellationToken)
        {
            Airport airport = new Airport()
            {
                Name = airportViewModel.Name,
                Latitude = airportViewModel.Latitude,
                Longitude = airportViewModel.Longitude
            };
            await airportManager.AddAirportAsync(airport, cancellationToken);
            return RedirectToAction("Index");
        }

        //GET : /Airport/EDIT
        public IActionResult Edit(Guid id)
        {
            Airport airport = airportManager.GetAirport(id);
            AirportViewModel airportViewModel = new AirportViewModel()
            {
                AirportId = airport.AirportId,
                Name = airport.Name,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude
            };
            return View(airportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AirportViewModel airportViewModel, CancellationToken cancellationToken)
        {
            Airport airport = new Airport()
            {
                AirportId = airportViewModel.AirportId,
                Name = airportViewModel.Name,
                Latitude = airportViewModel.Latitude,
                Longitude = airportViewModel.Longitude
            };
            await airportManager.UpdateAirportAsync(airport, cancellationToken);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            Airport airport = airportManager.GetAirport(id);
            AirportViewModel airportViewModel = new AirportViewModel()
            {
                AirportId = airport.AirportId,
                Name = airport.Name,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude
            };
            return View(airportViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            await airportManager.DeleteAirport(id, cancellationToken);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, CancellationToken cancellationToken)
        {
            string path = env.WebRootPath + "\\res\\AirportExcelFiles";
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        await airportManager.CreateAirport(stream, cancellationToken, path);
                    }
                }
            }
            return RedirectToAction("Index", "Airport");
        }
    }
}
