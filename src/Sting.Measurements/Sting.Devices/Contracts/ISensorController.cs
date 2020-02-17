using System.Threading.Tasks;
using Sting.Models;

namespace Sting.Devices.Contracts
{
    public interface ISensorController : IDevice
    {
        /// <summary>
        /// Takes a measurement with the installed Sensor.
        /// </summary>
        /// <returns>Returns the measured values inside
        /// a TelemetryData object.</returns>
        Task<MeasurementContainer> TakeMeasurement();
    }
}
