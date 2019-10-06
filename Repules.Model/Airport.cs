using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repules.Model
{
    public class Airport
    {

        public Guid AirportId { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ICollection<Flight> ArrivalFlights { get; private set; }

        public ICollection<Flight> DepartureFlights { get; private set; }

        public Airport()
        {
            ArrivalFlights = new HashSet<Flight>();
            DepartureFlights = new HashSet<Flight>();
        }
    }
}
