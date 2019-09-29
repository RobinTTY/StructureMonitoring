using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Sting.Persistence.Contracts;

namespace Sting.Persistence
{
    public class Database : IDatabase
    {
        private IMongoDatabase _database;
        
        private readonly string _clusterConnectionString;
        private readonly string _databaseName;

        // TODO: probably refactor, disconnect on shutdown?!
        public Database(string databaseName, string clusterConnectionString)
        {
            _databaseName = databaseName;
            _clusterConnectionString = clusterConnectionString;

            InitConnection();
        }

        public void InitConnection()
        {
            var client = new MongoClient(_clusterConnectionString);
            _database = client.GetDatabase(_databaseName);
        }

        public async Task SaveDocumentToCollection(BsonDocument document, string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            await collection.InsertOneAsync(document);
        }

        private void Ping()
        {
            _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
        }
    }
}
