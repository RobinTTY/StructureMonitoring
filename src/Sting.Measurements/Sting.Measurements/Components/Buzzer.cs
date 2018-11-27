using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Sting.Measurements.Components
{
    class Buzzer : IGpioComponent
    {
        private GpioPin _gpioPin;

        /// <inheritdoc />
        public async Task<bool> InitComponentAsync(int pin)
        {
            var gpio = await GpioController.GetDefaultAsync();

            if (gpio == null) return false;
            _gpioPin = gpio.OpenPin(pin);
            _gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            return true;
        }

        /// <inheritdoc />
        public bool State()
        {
            return _gpioPin != null;
        }

        /// <summary>
        /// Checks the current state of the Buzzer.
        /// </summary>
        /// <returns>Returns True if the LED is currently on.
        /// Returns False otherwise.</returns>
        public bool IsOn()
        {
            var state = _gpioPin.Read();
            return state == GpioPinValue.Low;
        }

        /// <inheritdoc />
        public void ClosePin()
        {
            _gpioPin.Dispose();
            _gpioPin = null;
        }

        /// <summary>
        /// Turns the Buzzer on.
        /// </summary>
        /// <returns>Returns True if the Buzzer was turned
        /// on successfully. Returns False otherwise.</returns>
        public bool TurnOn()
        {
            _gpioPin.Write(GpioPinValue.Low);
            return IsOn();
        }

        /// <summary>
        /// Turns the Buzzer off.
        /// </summary>
        /// <returns>Returns True if the Buzzer was turned
        /// off successfully. Returns False otherwise.</returns>
        public bool TurnOff()
        {
            _gpioPin.Write(GpioPinValue.High);
            return IsOn();
        }

        public void OnLocate(object source, EventArgs e)
        {
            if (!State()) return;
            TurnOn();
            Task.Delay(1000).Wait();
            TurnOff();
        }
    }
}
