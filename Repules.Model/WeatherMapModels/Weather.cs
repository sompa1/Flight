﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Repules.Model.WeatherMapModels
{
    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
