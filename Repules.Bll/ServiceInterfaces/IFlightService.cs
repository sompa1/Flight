using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repules.Model;

namespace Repules.Bll
{
    public interface IFlightService
    {
        Task AddFlightAsync(Flight flight, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteFlightAsync(Guid id, CancellationToken cancellationToken);
        Task<Flight> GetFlightAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Flight>> GetFlightsAsync(FlightStatus? flightStatus, CancellationToken cancellationToken);
        Task<Flight> GetUserFlightAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Flight>> GetUserFlightsAsync(CancellationToken cancellationToken);
        void ParseString(string line, Flight flight);
        Task SetAirportsAsync(Flight flight, CancellationToken cancellationToken = default(CancellationToken));
        void UpdateFlight(Flight flight);
    }
}