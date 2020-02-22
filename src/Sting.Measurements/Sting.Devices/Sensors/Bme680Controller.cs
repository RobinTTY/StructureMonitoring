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
            _bme680 = new Bme680(GetI2CDevice(config.I2CAddress));
            if (_bme680 == null)
                return false;

            SetDefaultConfiguration();
            _bme680.TemperatureSampling = config.TemperatureSampling;
            _bme680.PressureSampling = config.PressureSampling;
            _bme680.HumiditySampling = config.HumiditySampling;
            _bme680.FilterMode = config.FilteringMode;
            _bme680.GasConversionIsEnabled = config.GasConversionIsEnabled;
            _bme680.HeaterIsEnabled = config.HeaterIsEnabled;

            // Set heater profiles (cut off if more than 10)
            if (config.HeaterProfiles.Count > 10)
                config.HeaterProfiles.RemoveRange(10, config.HeaterProfiles.Count - 10);

            _bme680.TryReadTemperature(out var temperature);
            for (var i = 0; i < config.HeaterProfiles.Count; i++)
            {
                var heaterProfile = config.HeaterProfiles[i];
                _bme680.ConfigureHeatingProfile((Bme680HeaterProfile)i, heaterProfile.TargetTemperature, heaterProfile.Duration, temperature.Celsius);
            }

            _measurementDuration = _bme680.GetMeasurementDuration(_bme680.HeaterProfile);
            return true;
        }

        private I2cDevice GetI2CDevice(byte deviceAddress)
        {
            if (deviceAddress != Bme680.DefaultI2cAddress || deviceAddress != Bme680.SecondaryI2cAddress)
                return null;

            var i2CSettings = new I2cConnectionSettings(1, deviceAddress);
            return I2cDevice.Create(i2CSettings);
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

    internal class Bme680Configuration : IDeviceConfiguration
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

    internal class Bme680HeaterConfiguration
    {
        public ushort TargetTemperature { get; set; }
        public ushort Duration { get; set; }
    }
}
