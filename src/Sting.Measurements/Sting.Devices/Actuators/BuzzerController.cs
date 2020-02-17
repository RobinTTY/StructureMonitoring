using Iot.Device.Buzzer;
using Sting.Devices.Contracts;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : Buzzer, IDevice
    {
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }
    }
}
