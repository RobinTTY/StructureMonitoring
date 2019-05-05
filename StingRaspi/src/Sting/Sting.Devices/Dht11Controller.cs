using System.Device.I2c;
using System.Device.I2c.Drivers;
using Iot.Device.DHTxx;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class Dht11Controller : ISensorController
    {
        public DhtSensor _I2cdht11;

        public Dht11Controller()
        {
            var settings = new I2cConnectionSettings(1, DhtSensor.Dht12DefaultI2cAddress);
            var device = new UnixI2cDevice(settings);

            _I2cdht11 = new DhtSensor(device);
        }

        public MeasurementContainer TakeMeasurement()
        {
            double temperature = _I2cdht11.Temperature.Celsius;
            double humidity = _I2cdht11.Humidity;

            return new MeasurementContainer(temperature, humidity);
        }
    }
}
