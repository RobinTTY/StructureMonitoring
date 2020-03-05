﻿using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Bmp180;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configurations;

namespace Sting.Devices.Sensors
{
    public class Bmp180Controller : DeviceBase, ISensorController, IDisposable
    {
        private Bmp180 _bmp180;

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _bmp180.ReadTemperature().Celsius},
                    { "Pressure", _bmp180.ReadPressure().Pascal}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(Bmp180Configuration))
                return false;

            var config = (Bmp180Configuration)deviceConfiguration;
            var i2CSettings = new I2cConnectionSettings(1, config.I2CAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            
            // TODO: probably requires try catch?! Check device availability
            _bmp180 = new Bmp180(i2CDevice);
            _bmp180.SetSampling(config.Sampling);
            
            return true;
        }

        public void Dispose()
        {
            _bmp180?.Dispose();
            _bmp180 = null;
        }
    }
}
