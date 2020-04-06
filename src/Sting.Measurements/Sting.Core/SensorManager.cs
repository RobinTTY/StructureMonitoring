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
        private readonly IController _componentManager;
        private readonly ILogger _logger;
        private IDatabase _database;

        public SensorManager(IController componentManager, ILogger logger)
        {
            _sensors = new List<ISensorController>();
            _componentManager = componentManager;
            _logger = logger;
        }

        public void AddSensor(ISensorController sensorController) => _sensors.Add(sensorController);

        public void RemoveSensor(ISensorController sensorController) => _sensors.Remove(sensorController);

        public void Start()
        {
            State = State.Running;
            _database = _componentManager.GetDatabase();
            _sensors.AddRange(_componentManager.GetDevices().OfType<ISensorController>());

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

                measurement.Measurements.ToList().ForEach(kvp => _logger.Log($"{kvp.Key}: {kvp.Value}\n\n"));
            }).ToList();

            Task.WhenAll(tasks).Wait();
            var telemetry = new TelemetryData("testDevice", measurements.ToArray());
            _database.AddTelemetryData(telemetry);
        }
    }
}
