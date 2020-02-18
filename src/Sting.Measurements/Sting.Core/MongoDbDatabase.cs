using MongoDB.Bson;
using MongoDB.Driver;
using Sting.Core.Contracts;
using Sting.Models;

namespace Sting.Core
{
    public class MongoDbDatabase : IDatabase
    {
        public State State { get; set; }

        private IMongoDatabase _database;
        private readonly string _dbName;
        private readonly string _connectionString;

        // TODO: proper disconnect on shutdown?!
        public MongoDbDatabase(string databaseName, string connectionString)
        {
            _dbName = databaseName;
            _connectionString = connectionString;
        }

        // TODO: associate telemetry data class with collection through annotation if possible
        /// <summary>
        /// Saves <see cref="TelemetryData"/> to the database.
        /// </summary>
        /// <param name="telemetry">The <see cref="TelemetryData"/> to add.</param>
        public async void AddTelemetryData(TelemetryData telemetry)
        {
            // TODO: check state
            var collection = _database.GetCollection<BsonDocument>("TelemetryData");
            await collection.InsertOneAsync(telemetry.ToBsonDocument());
        }

        /// <summary>
        /// Pings the database, checking its availability.
        /// </summary>
        public void Ping()
        {
            _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
        }

        /// <summary>
        /// Initiates a connection with the MongoDB database.
        /// </summary>
        public void Start()
        {
            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_dbName);
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
