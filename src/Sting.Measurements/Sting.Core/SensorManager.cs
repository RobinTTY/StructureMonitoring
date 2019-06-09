using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Core
{
    public class SensorManager : ISensorManager
    {
        public bool IsRunning { get; set; }

        private readonly IEnumerable<ISensorController> _sensors;

        public SensorManager(IEnumerable<ISensorController> sensors)
        {
            _sensors = sensors;
        }

        public void Start()
        {
            IsRunning = true;

            Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    CollectSensorData();
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        // TODO: give ability to change period of collection
        private void CollectSensorData()
        {
            _sensors.ToList().ForEach(async sensor =>
            {
                var measurement = await sensor.TakeMeasurement();
                measurement.ToList().ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));
            });
            Task.Delay(1000).Wait();
        }

        // TODO: create a configure method to use custom configurations for the different sensors
    }
}
