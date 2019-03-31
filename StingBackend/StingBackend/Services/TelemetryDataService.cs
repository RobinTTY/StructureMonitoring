using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Sting.Backend.Filters;
using StingBackend.Models;

namespace Sting.Backend.Services
{
    public class TelemetryDataService
    {
        private readonly IMongoCollection<TelemetryData> _telemetryData;

        public TelemetryDataService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("StingDB"));
            var database = client.GetDatabase(config.GetSection("DatabaseInfo")["DatabaseName"]);

            // GetCollection<T> represents the CLR object type stored in the collection
            _telemetryData = database.GetCollection<TelemetryData>("TelemetryData");
        }

        public List<TelemetryData> Get()
        {
            return _telemetryData.Find(telemetryData => true).ToList();
        }

        public TelemetryData Get(string id)
        {
            return _telemetryData.Find(telemetryData => telemetryData.Id == id).FirstOrDefault();
        }

        public List<TelemetryData> GetDevice(string deviceName)
        {
            return _telemetryData.Find(telemetryData => telemetryData.DeviceName == deviceName).ToList();
        }

        public List<TelemetryData> Get(TelemetryDataFilter filter)
        {
            return _telemetryData
                .Find(telemetryData => true)
                .ToList();
        }

        public void Create(TelemetryData telemetryData)
        {
            _telemetryData.InsertOne(telemetryData);
        }

        public void Update(string id, TelemetryData telemetryDataIn)
        {
            _telemetryData.ReplaceOne(telemetryData => telemetryData.Id == id, telemetryDataIn);
        }

        public void Remove(TelemetryData telemetryDataIn)
        {
            _telemetryData.DeleteOne(telemetryData => telemetryData.Id == telemetryDataIn.Id);
        }

        public void Remove(string id)
        {
            _telemetryData.DeleteOne(telemetryData => telemetryData.Id == id);
        }
    }
}
