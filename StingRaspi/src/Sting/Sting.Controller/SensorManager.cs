using System.Collections.Generic;
using System.Linq;
using Sting.Devices;
using Sting.Models;

namespace Sting.Controller
{
    public class SensorManager : IService
    {
        public bool IsRunning { get; set; }

        private readonly IEnumerable<ISensorController> _sensors;

        public SensorManager(IEnumerable<ISensorController> sensors)
        {
            _sensors = sensors;
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void CollectSensorData()
        {
            var measurements = new List<MeasurementContainer>();

            _sensors.ToList().ForEach(sensor => measurements.Add(sensor.TakeMeasurement()));
        }
    }
}
