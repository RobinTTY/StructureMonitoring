using Iot.Device.Buzzer;
using Sting.Devices.Contracts;
using Sting.Models.Configuration;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : Buzzer, IDevice
    {
        public string DeviceName { get; set; }
        
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }

        public bool Configure(IDeviceConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }
    }
}
