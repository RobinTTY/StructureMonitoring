using System;
using System.Device.I2c;
using System.Device.I2c.Drivers;
using System.Threading.Tasks;
using Sting.Devices.Contracts;
using Sting.Models;
using Bme680Driver;

namespace Sting.Devices
{
    public class Bme680Controller : IBme680Controller
    {
        private readonly Bme680 _i2CBme680;

        public Bme680Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bme680.DefaultI2cAddress);
            // TODO: implement platform detection
            var i2CDevice = new UnixI2cDevice(i2CSettings);

            _i2CBme680 = new Bme680(i2CDevice);
            _i2CBme680.InitDevice();
        }

        public async Task<MeasurementContainer> TakeMeasurement()
        {
            await _i2CBme680.PerformMeasurement();

            var temp = _i2CBme680.Temperature;
            var humid = _i2CBme680.Humidity;
            var press = _i2CBme680.Pressure;
            var gasRes = _i2CBme680.GasResistance;

            return new MeasurementContainer(temp.Celsius, humid, press);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
