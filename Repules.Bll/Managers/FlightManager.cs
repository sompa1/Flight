using Repules.Dal;
using System;
using Repules.Model;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using System.Linq;

namespace Repules.Bll.Managers
{
    public class FlightManager
    {
        private readonly IFlightService flightService;
        private readonly IFlightLogFileService flightLogFileService;
        private readonly IGPSRecordService gPSRecordService;
        private readonly ApplicationContext applicationContext;

        public FlightManager(IFlightService flightService,
            IFlightLogFileService flightLogFileService,
            ApplicationContext applicationContext,
            IGPSRecordService gPSRecordService)
        {
            this.flightService = flightService;
            this.flightLogFileService = flightLogFileService;
            this.applicationContext = applicationContext;
            this.gPSRecordService = gPSRecordService;
        }

        public async Task ProcessFlightsAsync()
        {
            var uploadedFiles = flightLogFileService.GetUploadedFlightLogFiles();
            foreach (var flightLog in uploadedFiles) //amelyek még nem voltak feldolgozva
            {
                string path = flightLog.FilePath;
                try
                {   // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        Flight flight = new Flight();
                        flight.ApplicationUserId = flightLog.ApplicationUserId; //kinek a repülese
                        flight.FlightStatus = FlightStatus.WaitingForAcceptance;
                        while ((line = sr.ReadLine()) != null)
                        {
                            flightService.ParseString(line, flight);
                        }
                        await flightService.SetAirportsAsync(flight);
                        flightLog.FlightLogFileStatus = FlightLogFileStatus.Processed;
                        await flightService.AddFlightAsync(flight);
                        gPSRecordService.SetColor(flight);
                        var optimizedGPSRecords = GetApproximatingNodes(flight.GPSRecords.ToList());
                        foreach (var gpsRecord in optimizedGPSRecords)
                        {
                            flight.GPSRecords.Single(ent => ent.GPSRecordId == gpsRecord.GPSRecordId).IsOptimized = true; ;
                        }

                        await applicationContext.SaveChangesAsync();
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

        }

        private List<GPSRecord> GetApproximatingNodes(List<GPSRecord> fullSet, int targetCount = 7)
        {
            const double step = 0.0001; // the step by which bias is incremented.
            const int minModificationsPerRun = 20; // allows to increase bias even if modifications were performed.
            int strictBiasBound = targetCount * 16; // defines the bound after which optimisations are removed

            List<GPSRecord> a, b;
            a = fullSet;
            b = new List<GPSRecord>(fullSet.Count);
            int modified = 0;
            double bias = 0;
            double sum = 0;
            double avg = 1;

            while (a.Count > targetCount)
            {
                //Console.WriteLine(a.Count + "  bias: " + bias + " avg: " + avg);
                b.Add(a[0]);
                for (int i = 1; i < a.Count - 1; i++)
                {
                    double A = CalculateDistance(a[i - 1], a[i]);// distance too previus node 
                    double B = CalculateDistance(a[i + 1], a[i]);// distance to the next node
                    double C = CalculateDistance(a[i - 1], a[i + 1]);// distance between neighbouring nodes
                                                                        //removes nodes with the least impact on total length (favours removal of nodes with nodes closer than average)
                    if (A + B <= C * (1 + bias * (avg / C)))
                    {
                        modified++;
                        i++;
                        b.Add(a[i]);
                        sum += C;
                    }
                    else
                    {
                        b.Add(a[i]);
                        sum += A;
                    }
                }
                if (!b[b.Count - 1].Equals(a[a.Count - 1]))
                    b.Add(a[a.Count - 1]);

                if (modified < minModificationsPerRun && (b.Count > strictBiasBound || modified <= 1))
                {
                    bias += step;
                }
                else
                {
                    modified = 0;
                }
                //formula works better if avg value is less than true average; When list is small, correction for length is too great, thus avg value is locked at final stages
                avg = (a.Count >= strictBiasBound) ? (avg + avg + (sum / b.Count)) / 3 : avg;
                a = b;
                b = new List<GPSRecord>(a.Count);
            }
            return a;
        }

        public Task<List<Flight>> GetFlightsAsync(FlightStatus? flightStatus, CancellationToken cancellationToken)
        {
            return flightService.GetFlightsAsync(flightStatus, cancellationToken);
        }

        public async Task<Flight> GetFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            return await flightService.GetFlightAsync(id, cancellationToken);
        }

        public async Task<List<Flight>> GetUserFlightsAsync(CancellationToken cancellationToken)
        {
            return await flightService.GetUserFlightsAsync(cancellationToken);
        }

        public async Task<Flight> GetUserFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            return await flightService.GetUserFlightAsync(id, cancellationToken);
        }

        public async Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken)
        {
            flightService.UpdateFlight(flight);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddFlightAsync(Flight flight, CancellationToken cancellationToken)
        {
            await flightService.AddFlightAsync(flight, cancellationToken);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken)
        {
            await flightService.DeleteFlightAsync(id, cancellationToken);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        private double CalculateDistance(GPSRecord start, GPSRecord end)
        {
            var sCoord = new GeoCoordinate(start.Latitude, start.Longitude);
            var eCoord = new GeoCoordinate(end.Latitude, end.Longitude);
            return sCoord.GetDistanceTo(eCoord); //meterben adja vissza
        }

    }
}
