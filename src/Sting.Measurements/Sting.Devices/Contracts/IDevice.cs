namespace Sting.Devices.Contracts
{
    public interface IDevice
    {
        /// <summary>
        /// The unique name of the device.
        /// </summary>
        string DeviceName { get; }
    }
}
