using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Repules.Model;

namespace Repules.Bll
{
    public interface IFlightLogFileService
    {
        Task CreateLogFileAsync(Stream stream, CancellationToken cancellationToken);
        FlightLogFile GetFile(Guid id);
        List<FlightLogFile> GetUploadedFlightLogFiles();
    }
}