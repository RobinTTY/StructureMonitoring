using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sting.Controller.Contracts;
using Sting.Devices.Contracts;
using Sting.Models;

namespace Sting.Controller
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

            while (IsRunning)
            {
                ((ISi7021Controller) _sensors.Single(sensor => sensor is ISi7021Controller)).TurnOnHeater();
                CollectSensorData();
                Task.Delay(10000).Wait();
                ((ISi7021Controller)_sensors.Single(sensor => sensor is ISi7021Controller)).TurnOffHeater();
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void CollectSensorData()
        {
            var measurements = new List<MeasurementContainer>();

            _sensors.ToList().ForEach(sensor => measurements.Add(sensor.TakeMeasurement()));
            measurements.ForEach(measurement => Console.WriteLine($"Temperature: {measurement.Temperature}\nHumidity: {measurement.Humidity}\nPressure: {measurement.Pressure}\n"));
        }
    }
}
