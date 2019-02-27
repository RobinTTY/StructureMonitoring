using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using MongoDB.Bson;
using Sting.Devices;
using Sting.Storage;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Application
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private bool _cancelRequested;
        private string _deviceName;

        private readonly Bmp180 _bmp = new Bmp180(Resolution.UltraHighResolution);
        private readonly Dht11 _dht = new Dht11();
        private readonly Database _stingDatabase = new Database();
        
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _stingDatabase.InitConnection();
            _deferral = taskInstance.GetDeferral();
            InitComponentsAsync();
            Configure();
            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(20));
        }

        // initialize used components async
        private async void InitComponentsAsync()
        {
            await _bmp.InitializeAsync();
            await _dht.InitComponentAsync(4);
        }

        private void Configure()
        {
            var xmlFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "AppConfiguration.xml");
            var settings = XDocument.Load(xmlFilePath).Root?.Element("appSettings");

            _deviceName = settings?.Element("DeviceName")?.Value;
        }

        // Task which is executed every x seconds as defined in Run()
        // Take Measurements periodically
        private async void PeriodicTask(ThreadPoolTimer timer)
        {
            var data = await _bmp.ReadAsync();
            data.Complement(await _dht.TakeMeasurementAsync());
            Debug.WriteLine(data);

            var document = new BsonDocument()
            {
                { "DeviceName", _deviceName },
                {"TimeStamp", data.UnixTimeStampMilliseconds },
                {"Temperature", data.Temperature },
                {"Humidity", data.Humidity },
                {"Air Pressure", data.Pressure }
            };
            _stingDatabase.SaveDocumentToCollection(document, "TelemetryData");
        }
    }
}