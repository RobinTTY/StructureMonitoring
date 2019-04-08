﻿using System.Collections.Generic;
using Microsoft.AspNet.OData;
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
        [EnableQuery]
        public ActionResult<List<TelemetryData>> Get([FromQuery(Name = "TimeStampStart")] long? timeStampStart, [FromQuery(Name = "TimeStampStop")] long? timeStampStop)
        {
            var telemetryData = _telemetryDataService.Get(timeStampStart, timeStampStop);

            if (telemetryData == null)
                return NotFound();

            return telemetryData;
        }
    }
}