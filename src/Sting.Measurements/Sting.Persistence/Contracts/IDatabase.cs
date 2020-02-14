﻿using Sting.Models;

namespace Sting.Persistence.Contracts
{
    // TODO: maybe provide access trough local implementation of MongoDB
    public interface IDatabase
    {
        /// <summary>
        /// Adds new <see cref="TelemetryData"/> to the database.
        /// </summary>
        /// <param name="telemetry">The <see cref="TelemetryData"/> to add.</param>
        void AddTelemetryData(TelemetryData telemetry);
    }
}
