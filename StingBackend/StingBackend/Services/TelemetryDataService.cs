using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Sting.Backend.Models;

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
    }
}
