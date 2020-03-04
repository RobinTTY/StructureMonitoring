using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Reflection;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.DHTxx;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class DhtController : DeviceBase, ISensorController, IDisposable
    {
        private DhtBase _dht;

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer("Dht")
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _dht.Temperature.Celsius},
                    { "Humidity", _dht.Humidity}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(DhtConfiguration))
                return false;

            var config = (DhtConfiguration) deviceConfiguration;
            _dht = SelectDhtType(config);

            return _dht != null;
        }

        private DhtBase SelectDhtType(DhtConfiguration config)
        {
            var configTypeInstance = Activator.CreateInstance(config.DhtType, config.PinNumber);
            if (configTypeInstance.GetType() == typeof(DhtBase))
                return (DhtBase) configTypeInstance;

            return null;
        }

        public void Dispose()
        {
            _dht?.Dispose();
            _dht = null;
        }
    }
}
