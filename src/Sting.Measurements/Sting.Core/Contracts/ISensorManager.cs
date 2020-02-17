using Sting.Devices.Contracts;

namespace Sting.Core.Contracts
{
    public interface ISensorManager : IService
    {
        /// <summary>
        /// Adds a sensor to the manager.
        /// </summary>
        /// <param name="sensorController">The sensor to add.</param>
        void AddSensor(ISensorController sensorController);

        /// <summary>
        /// Removes a sensor from the manager.
        /// </summary>
        /// <param name="sensorController">The sensor to remove.</param>
        void RemoveSensor(ISensorController sensorController);
    }
}
