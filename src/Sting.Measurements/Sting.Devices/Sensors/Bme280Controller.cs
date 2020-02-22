using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class Bme280Controller : ISensorController, IDisposable
    {
        public string DeviceName { get; set; }

        private Bme280 _bme280;
        private readonly int _measurementDuration;

        public Bme280Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bmx280Base.SecondaryI2cAddress);
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

            var container = new MeasurementContainer("Bme280")
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", temperature.Celsius},
                    {"Humidity", humidity},
                    {"Pressure", pressure.Pascal}
                }
            };

            return Task.FromResult(container);
        }

        public bool Configure(IDeviceConfiguration configuration)
        {
            throw new NotImplementedException();
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
