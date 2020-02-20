using System.Collections.Generic;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;

namespace Sting.Core
{
    /// <summary>
    /// Manages the dynamic parts of the application that can be configured trough the front end
    /// </summary>
    public class DynamicComponentManager : IDynamicComponentManager
    {
        private readonly ILogger _logger;
        private List<IDevice> _devices;
        private IDatabase _database;
        

        public DynamicComponentManager(ILogger logger)
        {
            _logger = logger;
            _devices = new List<IDevice>();
        }

        /// <summary>
        /// Sets the currently used database for the application.
        /// </summary>
        /// <param name="database">The database which should be used.</param>
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

        /// <summary>
        /// Gets the currently registered database of the application.
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase() => _database;

        /// <summary>
        /// Registers a device with the application (e.g. a sensor)
        /// </summary>
        /// <param name="device">The <see cref="IDevice"/> to register.</param>
        public void AddDevice(IDevice device) => _devices.Add(device);

        /// <summary>
        /// Removes a device registration, making it unavailable to the application.
        /// </summary>
        /// <param name="device">The <see cref="IDevice"/> to remove.</param>
        public void RemoveDevice(IDevice device) => _devices.Remove(device);

        /// <summary>
        /// Gets the currently registered devices.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of the registered <see cref="IDevice"/>s.</returns>
        public List<IDevice> GetDevices() => _devices;
    }
}
