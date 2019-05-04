using System.Device.I2c;
using System.Device.I2c.Drivers;
using Iot.Device.Bmp180;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    class BMP180Controller : ISensorController
    {
        private readonly Bmp180 _I2cBmp180;

        public BMP180Controller()
        {
            var i2cSettings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            // TODO: test if this would work on Windows IoT Core - probably not so implement platform detection
            var i2cDevice = new UnixI2cDevice(i2cSettings);
            
            _I2cBmp180 = new Bmp180(i2cDevice);
        }

        // TODO: what happens if sampling mode is not set before taking first measurement?
        public void SetSamplingMode(Sampling samplingMode)
        {
            _I2cBmp180.SetSampling(samplingMode);
        }

        public MeasurementContainer TakeMeasurement()
        {
            double temperature = _I2cBmp180.ReadTemperature().Celsius;
            double pressure = _I2cBmp180.ReadPressure();
            
            return new MeasurementContainer(temperature, press: pressure);
        }
    }
}
