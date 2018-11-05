using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Sting.Cloud;
using Sting.Measurements.Components;

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
            InitComponents();
            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(5));
        }

        // initialize used components async
        // TODO: CHECK RETURN VALUE FOR SUCCESS
        private async void InitComponents()
        {
            await _tempSensor.InitComponentAsync(4);
            await _statusLed.InitComponentAsync(5);
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        // TODO: Introduce Cloud to Device Message which can cancel the deferral, resulting in termination of the program
        private async void PeriodicTask(ThreadPoolTimer timer)
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