using Repules.Dal;
using System;
using Repules.Model;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    }
}
