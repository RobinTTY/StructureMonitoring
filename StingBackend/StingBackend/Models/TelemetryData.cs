using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sting.Backend.Models
{
    public abstract class TelemetryData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("DeviceName")]
        public string DeviceName { get; set; }

        [BsonElement("TimeStamp")]
        public long UnixTimeStamp { get; set; }

        [BsonElement("Temperature")]
        public double Temperature { get; set; }

        [BsonElement("Humidity")]
        public double Humidity { get; set; }

        [BsonElement("AirPressure")]
        public double AirPressure { get; set; }
    }
}
