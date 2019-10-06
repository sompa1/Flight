using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repules.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public ICollection<Flight> Flights { get; set; }
        public ICollection<FlightLogFile> FlightLogFiles { get; set; }

        public ApplicationUser()
        {
            Flights = new HashSet<Flight>();
            FlightLogFiles = new HashSet<FlightLogFile>();
        }

    }
}
