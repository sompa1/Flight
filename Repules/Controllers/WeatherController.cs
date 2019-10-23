using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repules.Models;
using Repules.Bll;
using Repules.Model.WeatherMapModels;

namespace Repules.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET: Weather/SearchCity
        public IActionResult SearchCity()
        {
            var viewModel = new SearchCityViewModel();
            return View(viewModel);
        }

        // POST: Weather/SearchCity
        [HttpPost]
        public IActionResult SearchCity(SearchCityViewModel model)
        {
            // If the model is valid, consume the Weather API to bring the data of the city
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "Weather", new { city = model.CityName });
            }
            return View(model);
        }

        // GET: Weather/City
        public IActionResult City(string city)
        {
            // Consume the OpenWeatherAPI in order to bring Forecast data in our page.
            WeatherResponse weatherResponse = _weatherService.GetWeatherByName(city);
            CityViewModel viewModel = new CityViewModel();

            if (weatherResponse != null)
            {
                viewModel.Name = weatherResponse.Name;
                viewModel.Humidity = weatherResponse.Main.Humidity;
                viewModel.Pressure = weatherResponse.Main.Pressure;
                viewModel.Temp = weatherResponse.Main.Temp;
                viewModel.Weather = weatherResponse.Weather[0].Main;
                viewModel.Wind = weatherResponse.Wind.Speed;
                viewModel.Icon = weatherResponse.Weather[0].Icon;

            }
            return View(viewModel);
        }

    }
}