using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
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

        /// <summary>
        /// Saves a <see cref="BsonDocument"/> to the database.
        /// </summary>
        /// <param name="document">The document to save.</param>
        /// <param name="collectionName">The collection to save the document to.</param>
        /// <returns></returns>
        public async Task SaveDocumentToCollection(BsonDocument document, string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            await collection.InsertOneAsync(document);
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
