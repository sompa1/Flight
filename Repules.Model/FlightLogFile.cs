using System;
using System.Collections.Generic;
using System.Text;

namespace Repules.Model
{
    public class FlightLogFile
    {
        public Guid FlightLogFileId { get; set; }

        public string FilePath { get; set; }

        public Guid ApplicationUserId { get; set; } //ki töltötte fel a fájlt

        public ApplicationUser ApplicationUser { get; set; }

        public FlightLogFileStatus FlightLogFileStatus { get; set; }


    }
}
