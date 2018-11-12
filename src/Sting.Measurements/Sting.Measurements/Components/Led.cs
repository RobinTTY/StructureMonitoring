﻿using System;
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

        /// <inheritdoc />
        public bool State()
        {
            var state = _pin.Read();
            return state == GpioPinValue.Low;
        }

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
            return State();
        }

        /// <summary>
        /// Turns the LED off.
        /// </summary>
        /// <returns>Returns True if LED was successfully
        /// turned off. Returns false otherwise.</returns>
        public bool TurnOff()
        {
            _pin.Write(GpioPinValue.High);
            return State();
        }
    }
}
