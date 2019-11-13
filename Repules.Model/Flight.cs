using System;
using System.Collections.Generic;
using System.Text;

namespace Repules.Model
{

    public class Flight
    {
        public Guid FlightId { get; set; }
        public DateTime Date { get; set; }

        public int DurationHours { get; set; }

        public int DurationMins { get; set; }
        public int DurationSeconds { get; set; }

        public Guid DepartureLocationId { get; set; }

        public Airport DepartureLocation { get; set; }

        public Guid? ArrivalLocationId { get; set; }
        public Airport ArrivalLocation { get; set; }

        public FlightStatus FlightStatus { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<GPSRecord> GPSRecords { get; set; }
        public ICollection<GPSRecord> OptimizedGPSRecords { get; set; }

        public Flight()
        {
            GPSRecords = new HashSet<GPSRecord>();
        }

    }
}
