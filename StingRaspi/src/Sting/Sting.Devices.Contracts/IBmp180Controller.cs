using System;
using Iot.Device.Bmp180;

namespace Sting.Devices
{
    public interface IBmp180Controller : ISensorController, IDisposable
    {
        /// <summary>
        /// Sets the sampling mode of the Bmp180 sensor.
        /// </summary>
        /// <param name="samplingMode">The desired sampling mode.</param>
        void SetSamplingMode(Sampling samplingMode);
    }
}