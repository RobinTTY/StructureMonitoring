namespace Sting.Devices.Contracts
{
    public interface IDevice
    {
        /// <summary>
        /// The unique name of the device.
        /// </summary>
        string DeviceName { get; set; }

        /// <summary>
        /// Configures the device with the given configuration.
        /// </summary>
        /// <param name="jsonDeviceConfiguration">
        /// The device configuration as JSON string to use to configure the device.
        /// </param>
        /// <returns>
        /// True if the configuration is successfully applied.
        /// False otherwise.
        /// </returns>
        public bool Configure(string jsonDeviceConfiguration);
    }
}
