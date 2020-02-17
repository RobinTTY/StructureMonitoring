using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Bmp180;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class Bmp180Controller : ISensorController, IDisposable
    {
        private Bmp180 _i2CBmp180;

        public Bmp180Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            _i2CBmp180 = new Bmp180(i2CDevice);
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer("Bmp180")
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _i2CBmp180.ReadTemperature().Celsius},
                    { "Pressure", _i2CBmp180.ReadPressure().Pascal}
                }
            };

            return Task.FromResult(container);
        }

        public void Dispose()
        {
            _i2CBmp180?.Dispose();
            _i2CBmp180 = null;
        }
    }
}
