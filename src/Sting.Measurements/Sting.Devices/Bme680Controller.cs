using System;
using System.Device.I2c;
using System.Threading.Tasks;
using Sting.Devices.Contracts;
using Sting.Models;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;

namespace Sting.Devices
{
    public class Bme680Controller : ISensorController, IDisposable
    {
        private Bme680 _bme680;
        private readonly int _measurementDuration;

        public Bme680Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bme680.SecondaryI2cAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            _bme680 = new Bme680(i2CDevice);

            SetDefaultConfiguration();
            _measurementDuration = _bme680.GetMeasurementDuration(_bme680.HeaterProfile);
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            _bme680.SetPowerMode(Bme680PowerMode.Forced);
            Task.Delay(_measurementDuration).Wait();

            _bme680.TryReadTemperature(out var temperature);
            _bme680.TryReadHumidity(out var humidity);
            _bme680.TryReadPressure(out var pressure);
            _bme680.TryReadGasResistance(out var gasResistance);

            var measurements = new MeasurementContainer("Bme680")
            {
                {"Temperature", temperature.Celsius},
                {"Humidity", humidity},
                {"Pressure", pressure},
                {"GasResistance", gasResistance}
            };

            return Task.FromResult(measurements);
        }

        private void SetDefaultConfiguration()
        {
            _bme680.TemperatureSampling = Sampling.HighResolution;
            _bme680.PressureSampling = Sampling.HighResolution;
            _bme680.HumiditySampling = Sampling.HighResolution;
            _bme680.FilterMode = Bme680FilteringMode.C3;
        }

        public void Dispose()
        {
            _bme680?.Dispose();
            _bme680 = null;
        }
    }
}
