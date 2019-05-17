using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sting.Backend.Services;
using Sting.Models;

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
        public ActionResult<List<TelemetryData>> Get([FromQuery(Name = "TimeStampStart")] long? timeStampStart, [FromQuery(Name = "TimeStampStop")] long? timeStampStop, [FromQuery(Name = "DeviceId")] string deviceId)
        {
            var telemetryData = _telemetryDataService.Get(timeStampStart, timeStampStop, deviceId);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }

        [HttpGet("/latest")]
        public ActionResult<List<TelemetryData>> GetLatest([FromQuery(Name = "DeviceId")] string deviceId)
        {
            var telemetryData = _telemetryDataService.GetLatest(deviceId);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }
    }
}