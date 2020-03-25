using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Si7021;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices.Sensors
{
    public class Si7021Controller : DeviceBase, ISensorController, IDisposable
    {
        private Si7021 _si7021;

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _si7021.Temperature.Celsius},
                    { "Humidity", _si7021.Humidity}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(string jsonDeviceConfiguration)
        {
            var config = DeserializeDeviceConfig<Si7021Configuration>(jsonDeviceConfiguration);
            var i2CSettings = new I2cConnectionSettings(1, config.I2CAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);
            // TODO: probably requires try catch?! Check device availability

            _si7021 = new Si7021(i2CDevice)
            {
                Heater = config.HeaterIsEnabled, 
                Resolution = config.Resolution
            };

            return true;
        }

        public void Dispose()
        {
            _si7021.Heater = false;
            _si7021.Dispose();
            _si7021 = null;
        }
    }
}
