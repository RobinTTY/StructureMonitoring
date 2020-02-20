using System.Device.Gpio;
using System.Threading.Tasks;
using Sting.Devices.Contracts;

namespace Sting.Devices.Actuators
{
    // TODO: implement IDisposable to reset pin?
    public class LedController : IDevice
    {
        public string DeviceName { get; set; }
        public LedState State => CheckState();

        private readonly GpioController _gpioController;
        private readonly int _pin;

        public LedController(int pinNumber, GpioController controller)
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

    public enum LedState
    {
        On,
        Off
    }
}
