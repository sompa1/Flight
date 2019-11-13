using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repules.Models
{
    public class UserFlightViewModel
    {
        public Guid FlightId { get; set; }

        [Display(Name = "Repülés dátuma")]
        public string Date { get; set; }

        [Display(Name = "Repülés időtartama")]
        public string Duration { get; set; }

        [Display(Name = "Felszállás helye")]
        public string DepartureName { get; set; }

        [Display(Name = "Leszállás helye")]
        public string ArrivalName { get; set; }

        [Display(Name = "Státusz")]
        public string Status { get; set; }

        public string GPSRecords { get; set; }

        public string OptGPSRecords { get; set; }
    }

}
