using System;
using Newtonsoft.Json;

namespace Sting.Measurements
{
    class TelemetryData
    {
        public DateTime Timestamp;
        public double Temperature;
        public double Humidity;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
