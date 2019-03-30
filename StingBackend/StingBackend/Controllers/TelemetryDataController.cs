using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sting.Backend.Services;
using StingBackend.Models;

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
        public ActionResult<List<TelemetryData>> Get([FromQuery(Name = "TimeStampStart")] long? timeStampStart, [FromQuery(Name = "TimeStampStop")] long? timeStampStop)
        {
            var telemetryData = _telemetryDataService.Get(timeStampStart, timeStampStop);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }

        [HttpGet("{id:length(24)}", Name = "GetTelemetryData")]
        public ActionResult<TelemetryData> GetAll(string id)
        {
            var telemetryData = _telemetryDataService.Get(id);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }

        [HttpGet("/Device/{name}")]
        public ActionResult<List<TelemetryData>> GetDevice(string name)
        {
            var telemetryData = _telemetryDataService.GetDevice(name);

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