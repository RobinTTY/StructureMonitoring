﻿using MongoDB.Bson;

namespace Sting.Storage.Contracts
{
    public interface IDatabase
    {
        /// <summary>
        /// Initializes the connection to the database.
        /// </summary>
        void InitConnection();

        /// <summary>
        /// Saves a document to the database.
        /// </summary>
        /// <param name="document">The document to be saved.</param>
        /// <param name="collectionName">The collection which the document is saved to.</param>
        void SaveDocumentToCollection(BsonDocument document, string collectionName);
    }
}
