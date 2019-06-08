﻿using System;
using System.Device.I2c;
using System.Device.I2c.Drivers;
using System.Threading.Tasks;
using Iot.Device.Si7021;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices
{
    public class Si7021Controller : ISi7021Controller
    {
        private Si7021 _si7021;

        public Si7021Controller()
        {
            // TODO: maybe define a getI2CDevice method to centralize this process (I2CDeviceFactory)
            var i2CSettings = new I2cConnectionSettings(1, Si7021.DefaultI2cAddress);
            var i2CDevice = new UnixI2cDevice(i2CSettings);

            _si7021 = new Si7021(i2CDevice);
        }

        // TODO: move to config method
        public void SetResolution(Resolution resolution)
        {
            _si7021.Resolution = resolution;
        }

        // TODO: move to config method
        public void TurnOnHeater()
        {
            _si7021.Heater = true;
        }

        // TODO: move to config method
        public void TurnOffHeater()
        {
            _si7021.Heater = false;
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var temperature = _si7021.Temperature.Celsius;
            var humidity = _si7021.Humidity;
            
            return Task.FromResult(new MeasurementContainer(temperature, humidity));
        }

        public void Dispose()
        {
            _si7021.Heater = false;
            _si7021.Dispose();
            _si7021 = null;
        }
    }
}
