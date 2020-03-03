using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Sting.Devices.BaseClasses;

namespace Sting.Devices.Sensors
{
    public class Bme680Controller : DeviceBase, ISensorController, IDisposable
    {
        private Bme680 _bme680;
        private int _measurementDuration;

        public Task<MeasurementContainer> TakeMeasurement()
        {
            _bme680.SetPowerMode(Bme680PowerMode.Forced);
            Task.Delay(_measurementDuration).Wait();

            _bme680.TryReadTemperature(out var temperature);
            _bme680.TryReadHumidity(out var humidity);
            _bme680.TryReadPressure(out var pressure);
            _bme680.TryReadGasResistance(out var gasResistance);

            // TODO: probably use DeviceName
            var container = new MeasurementContainer(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", temperature.Celsius},
                    {"Humidity", humidity},
                    {"Pressure", pressure.Pascal},
                    {"GasResistance", gasResistance}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(Bme680Configuration))
                return false;

            var config = (Bme680Configuration)deviceConfiguration;
            var i2CSettings = new I2cConnectionSettings(1, config.I2CAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            // TODO: probably requires try catch?! Check device availability
            _bme680 = new Bme680(i2CDevice);
            
            SetDefaultConfiguration();
            SetPropertiesFromConfig(config);
            SetHeaterProfilesFromConfig(config);

            _measurementDuration = _bme680.GetMeasurementDuration(_bme680.HeaterProfile);
            return true;
        }

        private void SetPropertiesFromConfig(Bme680Configuration config)
        {
            _bme680.TemperatureSampling = config.TemperatureSampling;
            _bme680.PressureSampling = config.PressureSampling;
            _bme680.HumiditySampling = config.HumiditySampling;
            _bme680.FilterMode = config.FilteringMode;
            _bme680.GasConversionIsEnabled = config.GasConversionIsEnabled;
            _bme680.HeaterIsEnabled = config.HeaterIsEnabled;
        }

        private void SetHeaterProfilesFromConfig(Bme680Configuration config)
        {
            if (config.HeaterProfiles.Count > 10)
                config.HeaterProfiles.RemoveRange(10, config.HeaterProfiles.Count - 10);

            _bme680.TryReadTemperature(out var temperature);
            foreach (var heaterProfile in config.HeaterProfiles)
            {
                _bme680.ConfigureHeatingProfile(heaterProfile.HeaterProfile, heaterProfile.TargetTemperature,
                    heaterProfile.Duration, temperature.Celsius);
            }

            _bme680.HeaterProfile = config.ActiveProfile;
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
