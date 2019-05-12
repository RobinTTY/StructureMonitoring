using System.Device.I2c;
using System.Device.I2c.Drivers;
using Iot.Device.Bmp180;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class Bmp180Controller : IBmp180Controller
    {
        private readonly Bmp180 _i2CBmp180;

        public Bmp180Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            // TODO: test if this would work on Windows IoT Core - probably not so implement platform detection
            var i2CDevice = new UnixI2cDevice(i2CSettings);
            
            _i2CBmp180 = new Bmp180(i2CDevice);
        }

        // TODO: what happens if sampling mode is not set before taking first measurement?
        public void SetSamplingMode(Sampling samplingMode)
        {
            _i2CBmp180.SetSampling(samplingMode);
        }

        public MeasurementContainer TakeMeasurement()
        {
            var temperature = _i2CBmp180.ReadTemperature().Celsius;
            var pressure = _i2CBmp180.ReadPressure();
            
            return new MeasurementContainer(temperature, press: pressure);
        }

        public void Dispose()
        {
            _i2CBmp180?.Dispose();
        }
    }
}
