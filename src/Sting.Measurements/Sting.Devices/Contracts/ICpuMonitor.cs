namespace Sting.Devices.Contracts
{
    public interface ICpuMonitor : ISensorController
    {
        /// <summary>
        /// Indicates whether a temperature sensor is available for the CPU.
        /// </summary>
        bool SensorAvailable { get; }
    }
}
