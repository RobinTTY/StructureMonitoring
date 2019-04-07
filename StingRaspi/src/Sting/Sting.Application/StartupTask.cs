﻿using System;
using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Threading;
using MongoDB.Bson;
using Sting.Devices;
using Sting.Storage;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Sting.Application
{
    public sealed class StartupTask : IBackgroundTask
    {
        private readonly Bmp180 _bmp = new Bmp180(Resolution.UltraHighResolution);
        private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();
        private readonly Dht11 _dht = new Dht11();

        private readonly Database _stingDatabase = new Database();
        private bool _cancelRequested;
        private BackgroundTaskDeferral _deferral;
        private string _deviceName;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _stingDatabase.InitConnection();
            _deferral = taskInstance.GetDeferral();
            InitComponentsAsync();
            Configure();
            ThreadPoolTimer.CreatePeriodicTimer(PeriodicTask, TimeSpan.FromSeconds(10));
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