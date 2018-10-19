using System;
using System.Threading;
using Windows.Devices.Gpio;
using Sensors.Dht;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static IDht _dht = null;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            InitGpio();
            InitTimedMeasurement(20);
            //for (;;);
        }

        private static void InitGpio()
        {
            // Open a connection to the used GPIO pin 4 in exclusive mode
            const int gpioPin = 4;
            GpioPin dhtPin = GpioController.GetDefault().OpenPin(gpioPin, GpioSharingMode.Exclusive);
            _dht = new Dht11(dhtPin, GpioPinDriveMode.Input);
        }

        private static void InitTimedMeasurement(int period)
        {
            // Implement timer to perform a measurement every x seconds
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(period);
            var stateTimer = new Timer(TakeMeasurement, null, startTimeSpan, periodTimeSpan);
        }

        private static async void TakeMeasurement(object state)
        {
            // Take measurement and check for validity
            double temp = 0.0;
            double humidity = 0.0;
            DhtReading measurement = await _dht.GetReadingAsync().AsTask();
            if (measurement.IsValid)
            {
                temp = measurement.Temperature;
                humidity = measurement.Humidity;
            }
        }
    }
}
