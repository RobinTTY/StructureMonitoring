using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Sting.Models
{
    public class MeasurementContainer
    {
        [BsonElement("SensorName")]
        public string SensorName { get; }

        [BsonElement("Measurements")]
        public Dictionary<string, double> Measurements;

        public MeasurementContainer(string sensorName)
        {
            SensorName = sensorName;
        }
    }
}
