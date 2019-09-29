using Iot.Device.Buzzer;

namespace Sting.Devices
{
    public class BuzzerController : Buzzer
    {
        public BuzzerController(int pinNumber, int pwmChannel = -1) : base(pinNumber, pwmChannel) { }
    }
}
