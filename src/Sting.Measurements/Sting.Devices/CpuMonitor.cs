using System.Threading.Tasks;
using Iot.Device.CpuTemperature;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class CpuMonitor : ISensorController
    {
        private readonly CpuTemperature _monitor;

        public CpuMonitor()
        {
            _monitor = new CpuTemperature();
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var measurements = new MeasurementContainer("CpuMonitor") {{"Temperature", _monitor.Temperature.Celsius}};

            return Task.FromResult(measurements);
        }
    }
}
