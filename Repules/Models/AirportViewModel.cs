using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class AirportViewModel
    {

        public Guid AirportId { get; set; }
        [Display(Name = "Repülőtér neve")]
        public string Name { get; set; }

        [Display(Name = "Időjárás")]
        public string WeatherIcon { get; set; }

        public string WeatherMain { get; set; }

        [Display(Name = "Szélesség")]
        public double Latitude { get; set; }

        [Display(Name = "Hosszúság")]
        public double Longitude { get; set; }
    }
}
