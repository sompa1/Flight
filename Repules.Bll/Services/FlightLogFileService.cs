using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Repules.Dal;
using Repules.Model;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace Repules.Bll
{
    internal class FlightLogFileService : IFlightLogFileService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FlightLogFileService(ApplicationContext applicationContext, IHttpContextAccessor httpContextAccessor)
        {
            this.applicationContext = applicationContext;
            this._httpContextAccessor = httpContextAccessor;
        }

        public List<FlightLogFile> GetUploadedFlightLogFiles()
        {
            List<FlightLogFile> fileList = applicationContext.FlightLogFiles.Where(f => f.FlightLogFileStatus == FlightLogFileStatus.Uploaded).ToList();
            return fileList;
        }
        public FlightLogFile GetFile(Guid id)
        {
            FlightLogFile file = applicationContext.FlightLogFiles.SingleOrDefault(f => f.FlightLogFileId == id);
            return file;
        }

        public async Task CreateLogFileAsync(Stream stream, CancellationToken cancellationToken, string path)
        {
            string filepath = Path.Combine(path, Path.GetRandomFileName());
            using (var fileStream = File.Create(filepath))
            {
                stream.Seek(0, SeekOrigin.Begin); //az elejerol kezdve masolunk
                stream.CopyTo(fileStream);
            }
            FlightLogFile flightLogFile = new FlightLogFile();
            flightLogFile.FilePath = filepath;
            flightLogFile.FlightLogFileStatus = FlightLogFileStatus.Uploaded;
            Guid userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            flightLogFile.ApplicationUserId = userId;
            await applicationContext.FlightLogFiles.AddAsync(flightLogFile, cancellationToken);
        }

    }
}
