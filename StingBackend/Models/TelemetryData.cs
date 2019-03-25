using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StingBackend.Models
{
    public class TelemetryData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Timestamp")]
        public long UnixTimeStamp { get; set; }

        [BsonElement("Temperature")]
        public double Temperature { get; set; }

        [BsonElement("Humidity")]
        public double Humidity { get; set; }

        [BsonElement("Pressure")]
        public double Pressure { get; set; }
    }
}
