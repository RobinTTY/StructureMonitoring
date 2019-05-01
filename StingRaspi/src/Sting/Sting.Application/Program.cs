using System;
using Sting.Devices;
using Sting.Storage;
using MongoDB.Bson;

namespace Sting.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        private readonly Bmp180 _bmp = new Bmp180(Resolution.UltraHighResolution);
        private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();
        private readonly Dht11 _dht = new Dht11();

        private readonly Database _stingDatabase = new Database();
        private bool _cancelRequested;
        private BackgroundTaskDeferral _deferral;

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _stingDatabase.InitConnection();
            _deferral = taskInstance.GetDeferral();
            InitComponentsAsync();

            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(60));
        }

        // initialize used components async
        private async void InitComponentsAsync()
        {
            await _bmp.InitializeAsync();
            await _dht.InitComponentAsync(4);
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        private async void PeriodicTask(ThreadPoolTimer timer)
        {
            var bmpMeasurement = await _bmp.ReadAsync();
            var dhtMeasurement = await _dht.TakeMeasurementAsync();

            if (bmpMeasurement == null)
                return;

            bmpMeasurement.Humidity = dhtMeasurement.Humidity;
            // TODO: change to hardware id
            bmpMeasurement.DeviceId = _deviceInfo.FriendlyName;

            _stingDatabase.SaveDocumentToCollection(bmpMeasurement.ToBsonDocument(), "TelemetryData");
        }
    }
}