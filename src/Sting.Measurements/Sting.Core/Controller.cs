using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using System.Collections.Generic;

namespace Sting.Core
{
    /// <summary>
    /// Manages the dynamic parts of the application that can be configured through the front end
    /// </summary>
    public class Controller : IController
    {
        public string ControllerName { get; set; }

        private readonly ILogger _logger;
        private readonly List<IDevice> _devices;
        private IDatabase _database;

        public Controller(ILogger logger)
        {
            _logger = logger;
            _devices = new List<IDevice>();
        }

        public void SetDatabase(IDatabase database)
        {
            if (_database == null)
                _database = database;
            else
            {
                _database.Stop();
                _logger.Log($"Database of Type { _database.GetType() } stopped. Changing to new database.");
                _database = database;
            }
        }

        public IDatabase GetDatabase() => _database;

        public void AddDevice(IDevice device)
        {
            _devices.Add(device);
            _logger.Log($"{device.DeviceName} added to Controller.");
        }

        public void RemoveDevice(IDevice device)
        {
            _devices.Remove(device);
            _logger.Log($"{device.DeviceName} removed from Controller.");
        }

        public IEnumerable<IDevice> GetDevices() => _devices;
    }
}
