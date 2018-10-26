using Windows.Devices.Gpio;
using Sensors.Dht;

namespace Sting.Measurements
{
    class Dht11 : ISensor
    {
        private GpioPin _pin;

        public Dht11(int pin)
        {
            InitSensor(pin);
        }

        public bool InitSensor(int pin)
        {
            throw new System.NotImplementedException();
        }

        public bool State()
        {
            throw new System.NotImplementedException();
        }
    }
}
