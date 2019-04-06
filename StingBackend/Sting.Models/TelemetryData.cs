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

        [BsonElement("Temperature")]
        public double Temperature { get; set; }

        [BsonElement("Humidity")]
        public double Humidity { get; set; }

        [BsonElement("AirPressure")]
        public double AirPressure { get; set; }


        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="deviceId">The deviceId the telemetry was taken from.</param>
        /// <param name="temperature">temperature in °C.</param>
        /// <param name="humidity">humidity in percent.</param>
        /// <param name="pressure">absolute pressure.</param>
        public TelemetryData(string deviceId = "", double temperature = double.NaN, double humidity = double.NaN, double pressure = double.NaN)
        {
            Id = ObjectId.GenerateNewId();
            UnixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DeviceId = deviceId;
            Temperature = temperature;
            Humidity = humidity;
            AirPressure = pressure;
        }

        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            return "Temperature: " + Temperature + "°C, Humidity: " + Humidity + "%, Pressure: " + AirPressure + "hPa";
        }
    }
}
