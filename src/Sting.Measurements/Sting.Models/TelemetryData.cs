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

        [BsonElement("SensorData")]
        public SensorData[] SensorData { get; set; }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="deviceId">The deviceId the telemetry was taken from.</param>
        /// <param name="sensorData">The collected sensor data.</param>
        public TelemetryData(string deviceId, SensorData[] sensorData)
        {
            Id = ObjectId.GenerateNewId();
            UnixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DeviceId = deviceId;
            SensorData = sensorData;
        }
    }
}
