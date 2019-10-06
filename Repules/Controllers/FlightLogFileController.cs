using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repules.Bll.Managers;
using Repules.Model;
using Repules.Models;

namespace Repules.Controllers
{
    [Authorize(Roles = "user")]
    public class FlightLogFileController : Controller
    {

        public readonly FlightLogFileManager flightLogFileManager;

        public FlightLogFileController(FlightLogFileManager flightLogFileManager)
        {
            this.flightLogFileManager = flightLogFileManager;
        }

        public async Task<IActionResult> Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, CancellationToken cancellationToken)
        {
                                  
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        await flightLogFileManager.CreateLogFile(stream, cancellationToken);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
