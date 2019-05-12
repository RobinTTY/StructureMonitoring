using System.Device.Gpio;
using System.Threading.Tasks;
using Sting.Devices.Contracts;

namespace Sting.Devices
{
    public class Led : ILed
    {
        public LedState State => CheckState();

        private readonly IGpioController _gpioController;
        private readonly int _pin;

        public Led(int pinNumber, IGpioController controller)
        {
            _gpioController = controller;
            _pin = pinNumber;

            _gpioController.OpenPin(_pin, PinMode.Output);
            _gpioController.Write(_pin, PinValue.Low);
        }

        public void TurnOn()
        {
            _gpioController.Write(_pin, PinValue.High);
        }

        public void TurnOff()
        {
            _gpioController.Write(_pin, PinValue.Low);
        }

        public async Task BlinkAsync(int duration)
        {
            TurnOn();
            await Task.Delay(duration);
            TurnOff();
        }

        private LedState CheckState()
        {
            var state = _gpioController.Read(_pin);
            return state == PinValue.High ? LedState.On : LedState.Off;
        }
    }
}
