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
        public State State { get; set; }

        private readonly List<ISensorController> _sensors;
        private readonly IDynamicComponentManager _componentManager;
        private IDatabase _database;

        public SensorManager(IDynamicComponentManager componentManager)
        {
            _sensors = new List<ISensorController>();
            _componentManager = componentManager;
        }

        public void AddSensor(ISensorController sensorController) => _sensors.Add(sensorController);

        public void RemoveSensor(ISensorController sensorController) => _sensors.Remove(sensorController);

        public void Start()
        {
            State = State.Running;
            _database = _componentManager.GetDatabase();
            _sensors.AddRange(_componentManager.GetDevices()
                .Where(device => device.GetType().GetInterfaces().Contains(typeof(ISensorController)))
                .ToList().Cast<ISensorController>());

            Task.Factory.StartNew(() =>
            {
                while (State == State.Running)
                {
                    CollectSensorData();
                    Task.Delay(5000).Wait();
                }
            });
        }

        public void Stop()
        {
            State = State.Stopped;
        }

        private void CollectSensorData()
        {
            var measurements = new List<MeasurementContainer>();
            var tasks = _sensors.ToList().Select(async sensor =>
            {
                var measurement = await sensor.TakeMeasurement();
                measurements.Add(measurement);

                measurement.Measurements.ToList().ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));
                Console.WriteLine();
            }).ToList();

            Task.WhenAll(tasks).Wait();
            var telemetry = new TelemetryData("testDevice", measurements.ToArray());
            _database.AddTelemetryData(telemetry);
        }
    }
}
