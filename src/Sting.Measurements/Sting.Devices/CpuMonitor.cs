using System.Threading.Tasks;
using Iot.Device.CpuTemperature;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class CpuMonitor : ICpuMonitor
    {
        public bool SensorAvailable => _monitor.IsAvailable;
        private readonly CpuTemperature _monitor;

        public CpuMonitor()
        {
            _monitor = new CpuTemperature();
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            // TODO: new Measurement Container entirely or new concept?!
            var temperature = _monitor.Temperature.Celsius;

            return Task.FromResult(new MeasurementContainer(temperature));
        }
    }
}
