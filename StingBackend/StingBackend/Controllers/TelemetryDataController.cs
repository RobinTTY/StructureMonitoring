using System.Collections.Generic;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Sting.Backend.Models;
using Sting.Backend.Services;

namespace Sting.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryDataController : ControllerBase
    {
        private readonly TelemetryDataService _telemetryDataService;

        public TelemetryDataController(TelemetryDataService telemetryDataService)
        {
            _telemetryDataService = telemetryDataService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<List<TelemetryData>> Get()
        {
            var telemetryData = _telemetryDataService.Get();

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }
    }
}