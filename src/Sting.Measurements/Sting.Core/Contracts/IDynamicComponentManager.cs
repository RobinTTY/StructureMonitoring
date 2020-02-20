using System.Collections.Generic;
using Sting.Devices.Contracts;

namespace Sting.Core.Contracts
{
    public interface IDynamicComponentManager
    {
        /// <summary>
        /// Sets the currently used database for the application.
        /// </summary>
        /// <param name="database">The database which should be used.</param>
        void SetDatabase(IDatabase database);

        /// <summary>
        /// Gets the currently registered database of the application.
        /// </summary>
        /// <returns>The currently registered <see cref="IDatabase"/>.</returns>
        IDatabase GetDatabase();

        /// <summary>
        /// Registers a device with the application (e.g. a sensor)
        /// </summary>
        /// <param name="device">The <see cref="IDevice"/> to register.</param>
        void AddDevice(IDevice device);

        /// <summary>
        /// Removes a device registration, making it unavailable to the application.
        /// </summary>
        /// <param name="device">The <see cref="IDevice"/> to remove.</param>
        void RemoveDevice(IDevice device);

        /// <summary>
        /// Gets the currently registered devices.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of the registered <see cref="IDevice"/>s.</returns>
        List<IDevice> GetDevices();
    }
}