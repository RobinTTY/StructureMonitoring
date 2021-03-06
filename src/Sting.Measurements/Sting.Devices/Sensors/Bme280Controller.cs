﻿using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;

namespace Sting.Devices.Sensors
{
    public class Bme280Controller : DeviceBase, ISensorController, IDisposable
    {
        private Bme280 _bme280;
        private int _measurementDuration;

        public Bme280Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Bmx280Base.SecondaryI2cAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            _bme280 = new Bme280(i2CDevice);

            SetDefaultConfiguration();
            _measurementDuration = _bme280.GetMeasurementDuration();
        }

        public Task<SensorData> TakeMeasurement()
        {
            _bme280.SetPowerMode(Bmx280PowerMode.Forced);
            Task.Delay(_measurementDuration).Wait();

            _bme280.TryReadTemperature(out var temperature);
            _bme280.TryReadHumidity(out var humidity);
            _bme280.TryReadPressure(out var pressure);

            var container = new SensorData(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", temperature.DegreesCelsius},
                    {"Humidity", humidity},
                    {"Pressure", pressure.Pascals}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(string jsonDeviceConfiguration)
        {
            var config = DeserializeDeviceConfig<Bme280Configuration>(jsonDeviceConfiguration);
            var i2CSettings = new I2cConnectionSettings(1, config.I2CAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            // TODO: probably requires try catch?! Check device availability
            _bme280 = new Bme280(i2CDevice);

            SetDefaultConfiguration();
            SetPropertiesFromConfig(config);

            _measurementDuration = _bme280.GetMeasurementDuration();
            return true;
        }

        private void SetDefaultConfiguration()
        {
            _bme280.TemperatureSampling = Sampling.HighResolution;
            _bme280.HumiditySampling = Sampling.HighResolution;
            _bme280.PressureSampling = Sampling.HighResolution;
            _bme280.FilterMode = Bmx280FilteringMode.X2;
        }

        private void SetPropertiesFromConfig(Bme280Configuration config)
        {
            _bme280.TemperatureSampling = config.TemperatureSampling;
            _bme280.HumiditySampling = config.HumiditySampling;
            _bme280.PressureSampling = config.PressureSampling;
            _bme280.FilterMode = config.FilterMode;
            _bme280.StandbyTime = config.StandbyTime;
        }

        public void Dispose()
        {
            _bme280?.Dispose();
            _bme280 = null;
        }
    }
}
