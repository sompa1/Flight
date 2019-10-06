using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Repules.Model;

namespace Repules.Bll
{
    public interface IAirportService
    {
        Task AddAirportAsync(Airport airport, CancellationToken cancellationToken);
        Task CreateAirportAsync(Stream stream, CancellationToken cancellationToken);
        void DeleteAirport(Guid id);
        Airport GetAirport(Guid id);
        List<Airport> GetAirports();
        void UpdateAirport(Airport airport);
    }
}