using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using Windows.Devices.Gpio;
using Sensors.Dht;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Automation;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        
        private const int DhtPin = 4;
        private GpioPin _winDhtPin = null;
        private IDht _dht = null;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Open a connection to the used GPIO pin 4 in exclusive mode
            _winDhtPin = GpioController.GetDefault().OpenPin(DhtPin, GpioSharingMode.Exclusive);
            _dht = new Dht11(_winDhtPin, GpioPinDriveMode.Input);
            InitTimer();
            for (;;);
        }

        private static void InitTimer()
        {
            //implement timer to perform a measurement every x seconds
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(2);
            var stateTimer = new Timer((e) => { TakeMeasurement(); }, null, startTimeSpan, periodTimeSpan);
        }

        private static void TakeMeasurement()
        {

        }
    }
}
