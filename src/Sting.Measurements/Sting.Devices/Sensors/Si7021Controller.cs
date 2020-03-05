using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading.Tasks;
using Iot.Device.Si7021;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Models.Configuration;

namespace Sting.Devices.Sensors
{
    public class Si7021Controller : DeviceBase, ISensorController, IDisposable
    {

        private Si7021 _si7021;

        public Si7021Controller()
        {
            var i2CSettings = new I2cConnectionSettings(1, Si7021.DefaultI2cAddress);
            var i2CDevice = I2cDevice.Create(i2CSettings);

            _si7021 = new Si7021(i2CDevice);
        }

        public Task<MeasurementContainer> TakeMeasurement()
        {
            var container = new MeasurementContainer("Si7021")
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _si7021.Temperature.Celsius},
                    { "Humidity", _si7021.Humidity}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(IDeviceConfiguration deviceConfiguration)
        {
            if (deviceConfiguration.GetType() != typeof(Si7021Configuration))
                return false;

            var config = (Si7021Configuration)deviceConfiguration;
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
