using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Device.CpuTemperature;
using Sting.Devices.BaseClasses;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class CpuMonitor : DeviceBase, ISensorController
    {
        private readonly CpuTemperature _monitor;

        public CpuMonitor()
        {
            _monitor = new CpuTemperature();
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _monitor.Temperature.Celsius}
                }
            };

            return Task.FromResult(container);
        }

        /// <summary>
        /// Component does not require configuration.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IDeviceConfiguration"/>.</param>
        /// <returns>Always returns true.</returns>
        public override bool Configure(IDeviceConfiguration configuration) => true;
    }
}
