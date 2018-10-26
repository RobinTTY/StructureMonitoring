using Windows.Devices.Gpio;
using Sensors.Dht;

namespace Sting.Measurements
{
    class Dht11 : ISensor
    {
        private IDht _dht;
        GpioPin _gpioPin;

        public Dht11(int pin)
        {
            InitSensor(pin);
        }

        public bool InitSensor(int pin)
        {
            // Open the used GPIO pin, use as input
            var gpioController = GpioController.GetDefault();
            
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
    }
}
