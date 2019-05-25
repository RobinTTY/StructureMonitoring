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
            var measurements = new List<MeasurementContainer>();

            _sensors.ToList().ForEach(sensor => measurements.Add(sensor.TakeMeasurement()));
            measurements.ForEach(measurement => Console.WriteLine($"Temperature: {measurement.Temperature} Humidity: {measurement.Humidity}"));
            Task.Delay(1000).Wait();
        }
    }
}
