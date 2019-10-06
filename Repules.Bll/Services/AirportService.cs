using System;
using Repules.Dal;
using Repules.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Threading;
using System.Threading.Tasks;

namespace Repules.Bll
{
    internal class AirportService : IAirportService
    {

        private readonly ApplicationContext applicationContext;

        public AirportService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public List<Airport> GetAirports()
        {
            List<Airport> airportList = applicationContext.Airports.ToList();
            return airportList;
        }
        public Airport GetAirport(Guid id)
        {
            Airport airport = applicationContext.Airports.SingleOrDefault(a => a.AirportId == id);
            return airport;
        }

        public async Task AddAirportAsync(Airport airport, CancellationToken cancellationToken)
        {
            await applicationContext.Airports.AddAsync(airport);
        }

        public void UpdateAirport(Airport airport)
        {
            applicationContext.Airports.Update(airport);
        }

        public void DeleteAirport(Guid id)
        {
            Airport airport = applicationContext.Airports.SingleOrDefault(a => a.AirportId == id);
            if (airport == null)
                return;
            applicationContext.Airports.Remove(airport);
        }

        public async Task CreateAirportAsync(Stream stream, CancellationToken cancellationToken)
        {

            string path = Path.Combine(@"C:\Users\Panna\source\repos\projekt\Repules", Path.GetRandomFileName());
            using (var fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
            var fileInfo = new FileInfo(path);

            using (var excelPackage = new ExcelPackage(fileInfo))
            {
                var worksheet = excelPackage.Workbook.Worksheets.First();
                int rowCount = worksheet.Dimension.Rows;
                for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                {
                    await AddAirportAsync(new Airport
                    {
                        Name = worksheet.Cells[rowIndex, 1].Value.ToString(),
                        Latitude = Convert.ToDouble(worksheet.Cells[rowIndex, 2].Value.ToString()),
                        Longitude = Convert.ToDouble(worksheet.Cells[rowIndex, 3].Value.ToString())
                    }, cancellationToken);
                }

            }

        }


    }
}
