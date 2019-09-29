using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sting.Models
{
    public class TelemetryData
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("DeviceId")]
        public string DeviceId { get; set; }

        [BsonElement("TimeStamp")]
        public long UnixTimeStamp { get; set; }

        [BsonElement("MeasurementContainers")]
        public MeasurementContainer[] MeasurementContainers { get; set; }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="deviceId">The deviceId the telemetry was taken from.</param>
        /// <param name="containers">Containers of measurements.</param>
        public TelemetryData(string deviceId, MeasurementContainer[] containers)
        {
            Id = ObjectId.GenerateNewId();
            UnixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DeviceId = deviceId;
            MeasurementContainers = containers;
        }
    }
}
