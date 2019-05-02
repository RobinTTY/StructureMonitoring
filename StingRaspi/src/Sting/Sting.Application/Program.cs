using System;
using Sting.Storage;
using System.Configuration;
using System.Device.I2c;
using Iot.Device.Bmp180;
using MongoDB.Bson;
using Sting.Devices;

namespace Sting.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private readonly Database _stingDatabase = new Database();
        private readonly Bmp180 _bmp = new Bmp180();
        private readonly Dht11 _dht = new Dht11();

        // TODO: change to Net Core equivalent
        private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();
        
        private static void Main(string[] args)
        {
            // TODO: start the actual program
        }

        private void Configure()
        {
            _stingDatabase.InitConnection();
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
        private async void PeriodicTask()
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