using System.Collections.Generic;
using Sting.Controller.Contracts;
using Sting.Devices.Contracts;

namespace Sting.Controller
{
    public class SensorManager : IService
    {
        public bool IsRunning { get; set; }

        private readonly IEnumerable<ISensorDevice> _sensors;

        public SensorManager(IEnumerable<ISensorDevice> sensors)
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
    }
}
