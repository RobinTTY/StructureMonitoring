using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Sting.Devices.BaseClasses;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class DhtController : DeviceBase, ISensorController, IDisposable
    {
        private Dht11 _dht;

        public DhtController(int pinNumber)
        {
            _dht = new Dht11(pinNumber);
        }

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

        public override bool Configure(IDeviceConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dht?.Dispose();
            _dht = null;
        }
    }
}
