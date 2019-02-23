using System.Threading.Tasks;
using Sting.Units;

namespace Sting.Measurements.Contracts
{
    public interface ISensor
    {
        /// <summary>
        /// Takes a measurement with the installed Sensor.
        /// </summary>
        /// <returns>Returns the measured values inside
        /// a TelemetryData object.</returns>
        Task<TelemetryData> TakeMeasurementAsync();
    }
}
