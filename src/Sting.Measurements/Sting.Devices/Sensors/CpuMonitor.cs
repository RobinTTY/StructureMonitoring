﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iot.Device.CpuTemperature;
using Sting.Devices.BaseClasses;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Devices.Sensors
{
    public class CpuMonitor : DeviceBase, ISensorController
    {
        private readonly CpuTemperature _monitor;

        public CpuMonitor()
        {
            _monitor = new CpuTemperature();
        }

        public Task<SensorData> TakeMeasurement()
        {
            var container = new SensorData(DeviceName)
            {
                Measurements = new Dictionary<string, double>
                {
                    {"Temperature", _monitor.Temperature.DegreesCelsius}
                }
            };

            return Task.FromResult(container);
        }

        /// <summary>
        /// Component does not require configuration.
        /// </summary>
        /// <param name="configuration">empty</param>
        /// <returns>Always returns true.</returns>
        public override bool Configure(string configuration) => true;
    }
}
