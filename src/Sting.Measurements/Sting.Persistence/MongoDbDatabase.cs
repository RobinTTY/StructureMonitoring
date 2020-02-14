using MongoDB.Bson;
using MongoDB.Driver;
using Sting.Models;
using Sting.Persistence.Contracts;

namespace Sting.Persistence
{
    public class MongoDbDatabase : IDatabase
    {
        private IMongoDatabase _database;

        // TODO: proper disconnect on shutdown?!
        public MongoDbDatabase(string databaseName, string connectionString)
        {
            InitConnection(databaseName, connectionString);
        }

        /// <summary>
        /// Initiates a connection with the MongoDB database.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="connectionString">The connection string of the database.</param>
        public void InitConnection(string databaseName, string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        // TODO: associate telemetry data class with collection through annotation if possible
        /// <summary>
        /// Saves <see cref="TelemetryData"/> to the database.
        /// </summary>
        /// <param name="telemetry">The <see cref="TelemetryData"/> to add.</param>
        public async void AddTelemetryData(TelemetryData telemetry)
        {
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
    }
}
