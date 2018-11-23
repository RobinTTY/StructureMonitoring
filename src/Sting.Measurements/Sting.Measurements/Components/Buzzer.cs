using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Sting.Measurements.Components
{
    class Buzzer : IGpioComponent
    {
        private GpioPin _gpioPin;

        public async Task<bool> InitComponentAsync(int pin)
        {
            var gpio = await GpioController.GetDefaultAsync();

            if (gpio == null) return false;
            _gpioPin = gpio.OpenPin(pin);
            _gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            return true;
        }

        public bool State()
        {
            return _gpioPin != null;
        }

        public bool IsOn()
        {
            var state = _gpioPin.Read();
            return state == GpioPinValue.Low;
        }

        public void ClosePin()
        {
            _gpioPin.Dispose();
            _gpioPin = null;
        }

        public bool TurnOn()
        {
            _gpioPin.Write(GpioPinValue.Low);
            return IsOn();
        }

        public bool TurnOff()
        {
            _gpioPin.Write(GpioPinValue.High);
            return IsOn();
        }
    }
}
