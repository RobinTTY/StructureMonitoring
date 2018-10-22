using System;
using System.Diagnostics;
using Windows.Devices.Gpio;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Sensors.Dht;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const int DhtPin = 4;
        private BackgroundTaskDeferral _deferral;
        private static IDht _dht = null;
        private static Led _statusLed = new Led(5);
        volatile bool _cancelRequested = false;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            InitDht();
            _deferral = taskInstance.GetDeferral();
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TakeMeasurement, TimeSpan.FromSeconds(2));
        }

        private static void InitDht()
        {
            // Open the used GPIO pin 4
            GpioPin dhtPin = GpioController.GetDefault().OpenPin(DhtPin);
            _dht = new Dht11(dhtPin, GpioPinDriveMode.Input);
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
                    _statusLed.TurnOn();
                    Debug.WriteLine("Temp: " + temp + " Humidity: " + humidity);
                }
                else
                {
                    _statusLed.TurnOff();
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