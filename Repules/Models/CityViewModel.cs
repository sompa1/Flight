using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class CityViewModel
    {
        [Display(Name = "Város:")]
        public string Name { get; set; }

        [Display(Name = "Hőmérséklet:")]
        public float Temp { get; set; }

        [Display(Name = "Páratartalom")]
        public int Humidity { get; set; }

        [Display(Name = "Légnyomás:")]
        public int Pressure { get; set; }

        [Display(Name = "Szélsebesség:")]
        public float Wind { get; set; }

        [Display(Name = "Időjárási viszonylat:")]
        public string Weather { get; set; }

        [Display(Name = "Ikon:")]
        public string Icon { get; set; }
    }
}
