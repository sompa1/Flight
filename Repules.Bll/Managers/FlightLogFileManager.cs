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
    public class FlightLogFileManager
    {
        private readonly ApplicationContext applicationContext;
        private readonly IFlightLogFileService flightLogFileService;

        public FlightLogFileManager(ApplicationContext applicationContext, IFlightLogFileService flightLogFileService)
        {
            this.applicationContext = applicationContext;
            this.flightLogFileService = flightLogFileService;
        }
        public async Task CreateLogFile(Stream stream, CancellationToken cancellationToken)
        {
            await flightLogFileService.CreateLogFileAsync(stream, cancellationToken);
            await applicationContext.SaveChangesAsync(cancellationToken);
        }
    }
}
