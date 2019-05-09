using Sting.Models;

namespace Sting.Devices
{
    public interface ISensorController
    {
        /// <summary>
        /// Takes a measurement with the installed Sensor.
        /// </summary>
        /// <returns>Returns the measured values inside
        /// a TelemetryData object.</returns>
        MeasurementContainer TakeMeasurement();
    }
}
