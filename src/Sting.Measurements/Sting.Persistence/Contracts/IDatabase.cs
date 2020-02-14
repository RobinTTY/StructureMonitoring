using Sting.Models;

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

        /// <summary>
        /// Adds new <see cref="TelemetryData"/> to the database.
        /// </summary>
        /// <param name="telemetry">The <see cref="TelemetryData"/> to add.</param>
        void AddTelemetryData(TelemetryData telemetry);
    }
}
