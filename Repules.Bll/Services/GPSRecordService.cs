using System;
using Repules.Dal;
using Repules.Model;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;


namespace Repules.Bll
{
    internal class GPSRecordService : IGPSRecordService
    {

        public readonly ApplicationContext applicationContext;

        public GPSRecordService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public List<GPSRecord> GetGPSRecords()
        {
            List<GPSRecord> recordList = applicationContext.GPSRecords.ToList();
            return recordList;
        }

        public GPSRecord GetGPSRecord(Guid id)
        {
            GPSRecord record = applicationContext.GPSRecords.SingleOrDefault(g => g.GPSRecordId == id);
            return record;
        }

        public void UpdateGPSRecord(GPSRecord record)
        {
            applicationContext.GPSRecords.Update(record);
        }

        public void SetColor(Flight flight)
        {
            var minalt = flight.GPSRecords.OrderBy(r => r.Altitude).First(); //minimális magassag
            var maxalt = flight.GPSRecords.OrderBy(r => r.Altitude).Last(); //max magassag
            var GPSLogs = flight.GPSRecords;
            foreach (var record in GPSLogs)
            {
                Color c = MapRainbowColor(record.Altitude, minalt.Altitude, maxalt.Altitude);
                record.ColorA = c.A;
                record.ColorR = c.R;
                record.ColorG = c.G;
                record.ColorB = c.B;
                //UpdateGPSRecord(record);
            }
        }

        // Map a value to a rainbow color.
        private Color MapRainbowColor(float value, float red_value, float blue_value)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int)(1023 * (value - red_value) /
                (blue_value - red_value));

            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                return Color.FromArgb(255, int_value, 0);
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                return Color.FromArgb(255 - int_value, 255, 0);
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                return Color.FromArgb(0, 255, int_value);
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                return Color.FromArgb(0, 255 - int_value, 255);
            }
        }
    }
}
