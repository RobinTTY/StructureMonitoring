using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Sting.Measurements
{
    class TelemetryData
    {
        public DateTime Timestamp;
        public double Temperature;
        public double Humidity;

        public TelemetryData()
        {
            Timestamp = DateTime.Now;
            Temperature = 0.0;
            Humidity = 0.0;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return "Time: " + Timestamp.ToString(CultureInfo.InvariantCulture) + ", Temperature: " + Temperature.ToString(CultureInfo.InvariantCulture) + "°C, Humidity: " + Humidity.ToString(CultureInfo.InvariantCulture) + "%";
        }
    }
}
