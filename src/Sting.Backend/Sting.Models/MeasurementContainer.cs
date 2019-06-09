using System.Collections.Generic;

namespace Sting.Models
{
    // TODO: change measurement container to be a dictionary so it doesn't have to be changed, look at how DB needs to support this
    public class MeasurementContainer : Dictionary<string, double>
    {
        public string SensorName;
        
        public MeasurementContainer(string sensorName)
        {
            SensorName = sensorName;
        }
    }
}
