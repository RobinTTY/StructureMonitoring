using System;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class Bme280Controller : ISensorController, IDisposable
    {
        private Bme280 _bme280;
        private readonly int _measurementDuration;

        public Bme280Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bmx280Base.DefaultI2cAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            _bme280 = new Bme280(i2CDevice);

            SetDefaultConfiguration();
            _measurementDuration = _bme280.GetMeasurementDuration();
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            _bme280.SetPowerMode(Bmx280PowerMode.Forced);
            Task.Delay(_measurementDuration).Wait();

            _bme280.TryReadTemperature(out var temperature);
            _bme280.TryReadHumidity(out var humidity);
            _bme280.TryReadPressure(out var pressure);

            var measurements = new MeasurementContainer("Bme280")
            {
                {"Temperature", temperature.Celsius},
                {"Humidity", humidity},
                {"Pressure", pressure},
            };

            return Task.FromResult(measurements);
        }

        private void SetDefaultConfiguration()
        {
            _bme280.TemperatureSampling = Sampling.HighResolution;
            _bme280.HumiditySampling = Sampling.HighResolution;
            _bme280.PressureSampling = Sampling.HighResolution;
            _bme280.FilterMode = Bmx280FilteringMode.X2;
        }

        public void Dispose()
        {
            _bme280?.Dispose();
            _bme280 = null;
        }
    }
}
