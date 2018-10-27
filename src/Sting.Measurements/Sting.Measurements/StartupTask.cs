using System;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private readonly Led _statusLed = new Led();
        private readonly Dht11 _tempSensor = new Dht11();
        volatile bool _cancelRequested = false;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            _tempSensor.InitSensor(4);
            _statusLed.InitSensor(5);
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TakeMeasurement, TimeSpan.FromSeconds(2));
        }

        private void TakeMeasurement(ThreadPoolTimer timer)
        {
            if (_cancelRequested == false)
            {
                var telemetry = _tempSensor.TakeMeasurement();
                if (telemetry.Result == null) _statusLed.TurnOff();
                else _statusLed.TurnOn();
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