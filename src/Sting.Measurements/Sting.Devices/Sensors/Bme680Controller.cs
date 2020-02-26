﻿using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;

namespace Sting.Devices.Sensors
{
    public class Bme680Controller : ISensorController, IDisposable
    {
        public string DeviceName { get; set; }

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
            var container = new MeasurementContainer("Bme680")
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

        public bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(Bme680Configuration))
                return false;

            var config = (Bme680Configuration)deviceConfiguration;
            var i2CSettings = new I2cConnectionSettings(1, config.I2CAddress);
            var i2CDevice =  I2cDevice.Create(i2CSettings);
            
            _bme680 = new Bme680(i2CDevice);
            if (_bme680 == null)
                return false;

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
            for (var i = 0; i < config.HeaterProfiles.Count; i++)
            {
                var heaterProfile = config.HeaterProfiles[i];
                _bme680.ConfigureHeatingProfile((Bme680HeaterProfile)i, heaterProfile.TargetTemperature,
                    heaterProfile.Duration, temperature.Celsius);
            }
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

    public class Bme680Configuration : IDeviceConfiguration
    {
        public byte I2CAddress { get; set; }
        public bool HeaterIsEnabled { get; set; }
        public bool GasConversionIsEnabled { get; set; }
        public Sampling TemperatureSampling { get; set; }
        public Sampling PressureSampling { get; set; }
        public Sampling HumiditySampling { get; set; }
        public Bme680FilteringMode FilteringMode { get; set; }
        public List<Bme680HeaterConfiguration> HeaterProfiles { get; set; }
    }

    public class Bme680HeaterConfiguration
    {
        public ushort TargetTemperature { get; set; }
        public ushort Duration { get; set; }
    }
}
