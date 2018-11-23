using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Sting.Measurements.Components
{
    class Led : IGpioComponent
    {
        private GpioPin _pin;

        /// <inheritdoc />
        public async Task<bool> InitComponentAsync(int pin)
        {
            // Open the used GPIO pin and set as Output
            var gpio = await GpioController.GetDefaultAsync();

            if (gpio == null) return false;
            _pin = gpio.OpenPin(pin);
            _pin.SetDriveMode(GpioPinDriveMode.Output);
            return true;
        }

        /// <inheritdoc />
        public bool State()
        {
            return _pin != null;
        }

        /// <summary>
        /// Checks the current state of the LED.
        /// </summary>
        /// <returns>Returns True if the LED is currently on.
        /// Returns False otherwise.</returns>
        public bool IsOn()
        {
            var state = _pin.Read();
            return state == GpioPinValue.Low;
        }

        /// <inheritdoc />
        public void ClosePin()
        {
            _pin.Dispose();
            _pin = null;
        }

        /// <summary>
        /// Turns the LED on.
        /// </summary>
        /// <returns>Returns True if LED was successfully
        /// turned on. Returns false otherwise.</returns>
        public bool TurnOn()
        {
            _pin.Write(GpioPinValue.Low);
            return IsOn();
        }

        /// <summary>
        /// Turns the LED off.
        /// </summary>
        /// <returns>Returns True if LED was successfully
        /// turned off. Returns false otherwise.</returns>
        public bool TurnOff()
        {
            _pin.Write(GpioPinValue.High);
            return IsOn();
        }
    }
}
