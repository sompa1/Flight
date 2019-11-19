using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Repules.Model
{
    public class GPSRecord //a log fájlból beolvasott adatok
    {

        public Guid GPSRecordId { get; set; }

        public Guid? FlightId { get; set; }
        public Flight Flight { get; set; }

        public bool IsOptimized { get; set; }

        public DateTime TimeStamp { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Altitude { get; set; }

        public byte ColorA { get; set; }
        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }

        public override string ToString()
        {
            return "GPS rekord: időpont: " + TimeStamp.ToString("HH:mm:ss") + ", szélesség: " + Latitude + ", hosszúság: " + Longitude;
        }
    }
}
