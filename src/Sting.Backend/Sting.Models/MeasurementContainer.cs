using System.Collections.Generic;

namespace Sting.Models
{
    public class MeasurementContainer : Dictionary<string, double>
    {
        public string SensorName;
        
        public MeasurementContainer(string sensorName)
        {
            SensorName = sensorName;
        }
    }
}
