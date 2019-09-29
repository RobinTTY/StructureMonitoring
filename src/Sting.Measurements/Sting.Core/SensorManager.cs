using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using Sting.Models;
using Sting.Persistence.Contracts;

namespace Sting.Core
{
    public class SensorManager : ISensorManager
    {
        public bool IsRunning { get; set; }

        private readonly IEnumerable<ISensorController> _sensors;
        private readonly IDatabase _database;

        public SensorManager(IEnumerable<ISensorController> sensors, IDatabase database)
        {
            _sensors = sensors;
            _database = database;
        }

        public void Start()
        {
            IsRunning = true;

            Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    CollectSensorData();
                    Task.Delay(5000).Wait();
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        // TODO: give ability to change period of collection, only select period of upload -> always upload newest data
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
            _database.SaveDocumentToCollection(telemetry.ToBsonDocument(), "TelemetryData").Wait();
        }

        // TODO: create a configure method to use custom configurations for the different sensors
    }
}
