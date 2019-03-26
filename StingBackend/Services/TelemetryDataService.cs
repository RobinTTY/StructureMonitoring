using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using StingBackend.Models;

namespace StingBackend.Services
{
    public class TelemetryDataService
    {
        private readonly IMongoCollection<TelemetryData> _telemetryData;

        public TelemetryDataService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("StingMeasurements"));
            var database = client.GetDatabase("StingMeasurements");
            _telemetryData = database.GetCollection<TelemetryData>("Sting.TelemetryData");
        }

        public List<TelemetryData> Get()
        {
            return _telemetryData.Find(telemetryData => true).ToList();
        }

        public TelemetryData Get(string id)
        {
            return _telemetryData.Find<TelemetryData>(telemetryData => telemetryData.Id == id).FirstOrDefault();
        }

        public TelemetryData Create(TelemetryData telemetryData)
        {
            _telemetryData.InsertOne(telemetryData);
            return telemetryData;
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
