using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Sensors.Dht;

namespace Sting.Measurements.Components
{
    class Dht11 : IGpioComponent
    {
        private IDht _dht;
        GpioPin _gpioPin;

        public async Task<bool> InitComponentAsync(int pin)
        {
            // Open the used GPIO pin, use as input
            var gpioController = await GpioController.GetDefaultAsync();

            if (gpioController == null) return false;
            _gpioPin = gpioController.OpenPin(pin);
            _dht = new Sensors.Dht.Dht11(_gpioPin, GpioPinDriveMode.Input);
            return true;
        }

        public bool State()
        {
            // Check if _dht is null
            return _dht != null;
        }

        /// <summary>
        /// Takes a measurement of temperature and humidity. Adds the current timestamp.
        /// </summary>
        /// <returns>Returns TelemetryData if measurement is valid otherwise returns null.</returns>
        public async Task<TelemetryData> TakeMeasurement()
        {
            // Take measurement and check for validity
            var telemetry = new TelemetryData();
            var measurement = await _dht.GetReadingAsync().AsTask();

            if (!measurement.IsValid) return null;
            telemetry.Temperature = measurement.Temperature;
            telemetry.Humidity = measurement.Humidity;
            telemetry.Timestamp = DateTime.Now;
            return telemetry;
        }
    }
}
