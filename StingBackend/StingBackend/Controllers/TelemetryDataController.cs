using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<List<TelemetryData>> Get()
        {
            return _telemetryDataService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetTelemetryData")]
        public ActionResult<TelemetryData> Get(string id)
        {
            var telemetryData = _telemetryDataService.Get(id);

            if (telemetryData == null)
            {
                return NotFound();
            }

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