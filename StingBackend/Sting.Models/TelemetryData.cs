using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sting.Models
{
    public abstract class TelemetryData : BsonDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

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
    }
}
