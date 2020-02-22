using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Device.CpuTemperature;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class CpuMonitor : ISensorController
    {
        public string DeviceName { get; set; }

        private readonly CpuTemperature _monitor;

        public CpuMonitor()
        {
            _monitor = new CpuTemperature();
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer("CpuMonitor")
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _monitor.Temperature.Celsius}
                }
            };

            return Task.FromResult(container);
        }

        public bool Configure(IDeviceConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}
