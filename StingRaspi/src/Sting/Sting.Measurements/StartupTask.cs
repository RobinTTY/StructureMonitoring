using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Sting.Storage;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Measurements
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private bool _cancelRequested;
        private Database _stingDatabase = new Database();
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _stingDatabase.InitConnection();
            //_deferral = taskInstance.GetDeferral();
            //ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(60));
        }

        // initialize used components async
        private async void InitComponentsAsync()
        {
            // TODO: init
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        private async void PeriodicTask(ThreadPoolTimer timer)
        {
            // TODO: clean procedure
        }

        private void OnTerminate(object source, EventArgs e)
        {
            Debug.WriteLine("Termination was requested. Shutting Down.");
            _cancelRequested = true;
        }
    }
}