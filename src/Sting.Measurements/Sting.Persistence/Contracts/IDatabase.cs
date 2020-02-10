using System.Threading.Tasks;
using MongoDB.Bson;

namespace Sting.Persistence.Contracts
{
    // TODO: maybe provide access trough local implementation of MongoDB
    public interface IDatabase
    {
        /// <summary>
        /// Initiates a connection with the database.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="connectionString">The connection string of the database.</param>
        void InitConnection(string databaseName, string connectionString);

        // TODO: refactor for generalized db usage
        /// <summary>
        /// Saves a document to the database.
        /// </summary>
        /// <param name="document">The document to be saved.</param>
        /// <param name="collectionName">The collection which the document is saved to.</param>
        Task SaveDocumentToCollection(BsonDocument document, string collectionName);
    }
}
