using System.Device.Gpio;
using System.Threading.Tasks;

namespace Sting.Devices
{
    public class Led : ILed
    {
        public LedState State => CheckState();

        private readonly IGpioController _controller;
        private readonly int _pin;

        public Led(int pinNumber, IGpioController controller)
        {
            _controller = controller;
            _pin = pinNumber;

            _controller.OpenPin(_pin, PinMode.Output);
            _controller.Write(_pin, PinValue.Low);
        }

        public void TurnOn()
        {
            _controller.Write(_pin, PinValue.High);
        }

        public void TurnOff()
        {
            _controller.Write(_pin, PinValue.Low);
        }

        public async Task BlinkAsync(int duration)
        {
            TurnOn();
            await Task.Delay(duration);
            TurnOff();
        }

        private LedState CheckState()
        {
            var state = _controller.Read(_pin);
            return state == PinValue.High ? LedState.On : LedState.Off;
        }
    }
}
