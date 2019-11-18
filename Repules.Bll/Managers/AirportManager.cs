using Repules.Dal;
using System;
using Repules.Model;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Repules.Bll.Managers
{
    public class AirportManager
    {
        private readonly ApplicationContext applicationContext;
        private readonly IAirportService airportService;

        public AirportManager(ApplicationContext applicationContext, IAirportService airportService)
        {
            this.applicationContext = applicationContext;
            this.airportService = airportService;
        }

        public AirportManager(IAirportService airportService)
        {
            this.airportService = airportService;
        }

        public List<Airport> GetAirports()
        {
            return airportService.GetAirports();
        }

        public Airport GetAirport(Guid id)
        {
            return airportService.GetAirport(id);
        }

        public async Task AddAirportAsync(Airport airport, CancellationToken cancellationToken)
        {
            await airportService.AddAirportAsync(airport, cancellationToken);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAirportAsync(Airport airport, CancellationToken cancellationToken)
        {
            airportService.UpdateAirport(airport);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateAirport(Stream stream, CancellationToken cancellationToken, string path)
        {
            await airportService.CreateAirportAsync(stream, cancellationToken, path);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAirport(Guid id, CancellationToken cancellationToken)
        {
            airportService.DeleteAirport(id);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }
    }
}
