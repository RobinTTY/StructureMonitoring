using System;
using System.Collections.Generic;
using System.Text;

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
