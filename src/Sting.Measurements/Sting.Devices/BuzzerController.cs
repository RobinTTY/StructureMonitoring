using Iot.Device.Buzzer;
using Sting.Devices.Contracts;

namespace Sting.Devices
{
    public class BuzzerController : Buzzer, IBuzzerController
    {
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }
    }
}
