using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class DhtController : IDhtController
    {
        private Dht11 _dht;

        public DhtController(int pinNumber)
        {
            _dht = new Dht11(pinNumber);
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var measurements = new MeasurementContainer("Dht")
            {
                {"Temperature", _dht.Temperature.Celsius},
                { "Humidity", _dht.Humidity}
            };

            return Task.FromResult(measurements);
        }

        public void Dispose()
        {
            _dht?.Dispose();
            _dht = null;
        }
    }
}
