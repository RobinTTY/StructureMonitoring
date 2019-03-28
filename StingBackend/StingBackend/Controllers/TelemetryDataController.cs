using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StingBackend.Models;
using StingBackend.Services;

namespace StingBackend.Controllers
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
        public ActionResult<List<TelemetryData>> Get([FromQuery(Name = "TimeStampStart")] long? timeStampStart, [FromQuery(Name = "TimeStampStop")] long? timeStampStop)
        {
            List<TelemetryData> telemetryData;

            if(timeStampStart != null && timeStampStop != null)
                telemetryData = _telemetryDataService.Get((long) timeStampStart, (long) timeStampStop);
            else
                telemetryData = _telemetryDataService.Get();

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }

        [HttpGet("{id:length(24)}", Name = "GetTelemetryData")]
        public ActionResult<TelemetryData> Get(string id)
        {
            var telemetryData = _telemetryDataService.Get(id);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }

        [HttpPost]
        public ActionResult<TelemetryData> Create(TelemetryData telemetryData)
        {
            _telemetryDataService.Create(telemetryData);

            return CreatedAtRoute("GetTelemetryData", new { id = telemetryData.Id }, telemetryData);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, TelemetryData telemetryDataIn)
        {
            var telemetryData = _telemetryDataService.Get(id);

            if (telemetryData == null)
            {
                return NotFound();
            }

            _telemetryDataService.Update(id, telemetryDataIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var telemetryData = _telemetryDataService.Get(id);

            if (telemetryData == null)
            {
                return NotFound();
            }

            _telemetryDataService.Remove(telemetryData.Id);

            return NoContent();
        }
    }
}