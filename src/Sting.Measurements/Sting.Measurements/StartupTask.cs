using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.Threading;
using Sting.Cloud;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private readonly Led _statusLed = new Led();
        private readonly Dht11 _tempSensor = new Dht11();
        private readonly AzureIotHub _structureMonitoringHub = new AzureIotHub();
        volatile bool _cancelRequested = false;
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            _tempSensor.InitSensor(4);
            _statusLed.InitSensor(5);
            ThreadPoolTimer.CreatePeriodicTimer(TakeMeasurement, TimeSpan.FromSeconds(5));
        }

        private async void TakeMeasurement(ThreadPoolTimer timer)
        {
            if (_cancelRequested == false)
            {
                var telemetry = _tempSensor.TakeMeasurement();
                if (telemetry.Result == null)
                {
                    _statusLed.TurnOff();
                    Debug.WriteLine("Invalid Read");
                }
                else
                {
                    _statusLed.TurnOn();
                    var success = await _structureMonitoringHub.SendDeviceToCloudMessageAsync(telemetry.Result.ToJson());
                    Debug.WriteLine(success ? "Message sent!" : "Could not send you message.");
                }
                telemetry.Dispose();
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