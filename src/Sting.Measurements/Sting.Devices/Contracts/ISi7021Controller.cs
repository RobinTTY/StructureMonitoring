using System;
using System.Collections.Generic;
using System.Text;
using Iot.Device.Si7021;

namespace Sting.Devices.Contracts
{
    public interface ISi7021Controller : ISensorController, IDisposable
    {
        /// <summary>
        /// Sets the desired resolution to be used.
        /// </summary>
        /// <param name="resolution">The desired resolution.</param>
        void SetResolution(Resolution resolution);

        /// <summary>
        /// Turns on the integrated resistive heating element.
        /// </summary>
        void TurnOnHeater();

        /// <summary>
        /// Turns off the integrated resistive heating element.
        /// </summary>
        void TurnOffHeater();
    }
}
