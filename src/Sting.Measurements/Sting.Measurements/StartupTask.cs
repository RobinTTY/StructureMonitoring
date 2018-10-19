using System;
using Windows.Devices.Gpio;
using Sensors.Dht;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const int DhtPin = 4;
        private const int LedPin = 5;
        private BackgroundTaskDeferral _deferral;
        private GpioPin _led;
        private static IDht _dht = null;
        volatile bool _cancelRequested = false;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            InitDht();
            InitLed();
            _deferral = taskInstance.GetDeferral();
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TakeMeasurement, TimeSpan.FromSeconds(2));
        }

        private static void InitDht()
        {
            // Open the used GPIO pin 4
            GpioPin dhtPin = GpioController.GetDefault().OpenPin(DhtPin);
            _dht = new Dht11(dhtPin, GpioPinDriveMode.Input);
        }

        private void InitLed()
        {
            // Open the used GPIO pin 5 and set LED to off
            _led = GpioController.GetDefault().OpenPin(LedPin);
            _led.Write(GpioPinValue.High);
            _led.SetDriveMode(GpioPinDriveMode.Output);
        }

        private async void TakeMeasurement(ThreadPoolTimer timer)
        {
            // Take measurement and check for validity, indicate through LED
            if (_cancelRequested == false)
            {
                double temp = 0.0;
                double humidity = 0.0;
                DhtReading measurement = await _dht.GetReadingAsync().AsTask();

                if (measurement.IsValid)
                {
                    temp = measurement.Temperature;
                    humidity = measurement.Humidity;
                    _led.Write(GpioPinValue.Low);
                }
                else
                {
                    _led.Write(GpioPinValue.High);
                }
            }
            else
            {
                // indicate that deferral is completed
                timer.Cancel();
                _deferral.Complete();
            }
        }
    }
}
