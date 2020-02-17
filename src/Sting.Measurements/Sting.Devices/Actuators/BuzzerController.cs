using Iot.Device.Buzzer;

namespace Sting.Devices.Actuators
{
    public class BuzzerController : Buzzer
    {
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }
    }
}
