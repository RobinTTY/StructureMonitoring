using MongoDB.Bson;
using MongoDB.Driver;
using Sting.Persistence.Contracts;

namespace Sting.Persistence
{
    public class Database : IDatabase
    {
        private string _clusterConnectionString;
        private string _databaseName;
        private IMongoDatabase _database;

        public Database()
        {
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            _clusterConnectionString = "";
            _databaseName = "Sting";
        }

        public void InitConnection()
        {
            var client = new MongoClient(_clusterConnectionString);
            _database = client.GetDatabase(_databaseName);
        }

        public void SaveDocumentToCollection(BsonDocument document, string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var result = collection.InsertOneAsync(document);
        }

        private void Ping()
        {
            _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
        }
    }
}
