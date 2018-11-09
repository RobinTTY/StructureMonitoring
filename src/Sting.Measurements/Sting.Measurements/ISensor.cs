using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sting.Measurements
{
    interface ISensor
    {
        /// <summary>
        /// Takes a measurement with the installed Sensor.
        /// </summary>
        /// <returns>Returns the measured values inside
        /// a TelemetryData object.</returns>
        Task<TelemetryData> TakeMeasurement();
    }
}
