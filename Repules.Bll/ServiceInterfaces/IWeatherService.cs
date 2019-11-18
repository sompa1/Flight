using System;
using System.Collections.Generic;
using System.Text;
using Repules.Model.WeatherMapModels;

namespace Repules.Bll
{
    public interface IWeatherService
    {
        WeatherResponse GetWeatherByName(string city);
        WeatherResponse GetWeatherByCoords(double lat, double lon);
    }
}
