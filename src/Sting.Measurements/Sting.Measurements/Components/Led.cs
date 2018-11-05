using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Sting.Measurements.Components
{
    class Led : IGpioComponent
    {
        private GpioPin _pin;

        public async Task<bool> InitComponentAsync(int pin)
        {
            // Open the used GPIO pin and set as Output
            var ledPin = pin;
            var gpio = await GpioController.GetDefaultAsync();

            if (gpio == null)
            {
                _pin = null;
                return false;
            }
            _pin = gpio.OpenPin(ledPin);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
            return true;
        }

        public bool State()
        {
            var state = _pin.Read();
            if (state == GpioPinValue.Low) return true;
            return false;
        }

        public bool TurnOn()
        {
            _pin.Write(GpioPinValue.Low);
            return State();
        }

        public bool TurnOff()
        {
            _pin.Write(GpioPinValue.High);
            return State();
        }
    }
}
