using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Sting.Models
{
    public class SensorData
    {
        [BsonElement("SensorName")]
        public string SensorName { get; }

        [BsonElement("Measurements")]
        public Dictionary<string, double> Measurements;

        public SensorData(string sensorName)
        {
            SensorName = sensorName;
        }
    }
}
