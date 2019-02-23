using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Sting.Devices;
using Sting.Storage;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Application
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private bool _cancelRequested;

        private Bmp180 _bmp = new Bmp180(Resolution.UltraHighResolution);

        private readonly Database _stingDatabase = new Database();
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //_stingDatabase.InitConnection();
            _deferral = taskInstance.GetDeferral();
            InitComponentsAsync();
            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(10));
        }

        // initialize used components async
        private async void InitComponentsAsync()
        {
            await _bmp.InitializeAsync();
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        private async void PeriodicTask(ThreadPoolTimer timer)
        {
            var data = await _bmp.ReadAsync();
            Debug.WriteLine("Pressure: "+ data.Pressure + " Temperature: " + data.Temperature);
        }

        private void OnTerminate(object source, EventArgs e)
        {
            Debug.WriteLine("Termination was requested. Shutting Down.");
            _cancelRequested = true;
        }
    }
}