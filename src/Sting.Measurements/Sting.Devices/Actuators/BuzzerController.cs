using Iot.Device.Buzzer;
using Sting.Devices.Contracts;
using Sting.Models.Configuration;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : Buzzer, IDevice
    {
        public string DeviceName { get; set; }
        
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }

        /// <summary>
        /// The buzzer requires no further configuration and will always return true.
        /// </summary>
        /// <param name="configuration">The configuration parameters (none).</param>
        /// <returns>Returns true.</returns>
        public bool Configure(IDeviceConfiguration configuration) => true;
    }
}
