﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Sting.Devices.BaseClasses;
using Sting.Devices.Configurations;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices.Sensors
{
    public class DhtController : DeviceBase, ISensorController, IDisposable
    {
        private DhtBase _dht;

        public Task<SensorData> TakeMeasurement()
        {
            var container = new SensorData(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _dht.Temperature.DegreesCelsius},
                    { "Humidity", _dht.Humidity}
                }
            };

            return Task.FromResult(container);
        }

        public override bool Configure(string jsonDeviceConfiguration)
        {
            var config = DeserializeDeviceConfig<DhtConfiguration>(jsonDeviceConfiguration);
            _dht = GetDhtInstance(config);

            return _dht != null;
        }

        private DhtBase GetDhtInstance(DhtConfiguration config)
        {
            if (config.DhtType != typeof(DhtBase))
                return null;

            return (DhtBase)Activator.CreateInstance(config.DhtType, config.PinNumber);
        }

        public void Dispose()
        {
            _dht?.Dispose();
            _dht = null;
        }
    }
}
