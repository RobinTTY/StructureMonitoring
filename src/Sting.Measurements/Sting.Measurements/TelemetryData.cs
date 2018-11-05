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
        public double Pressure;

        public TelemetryData()
        {
            Timestamp = DateTime.Now;
            Temperature = 0.0;
            Humidity = 0.0;
            Pressure = 0.0;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return "Time: " + Timestamp + ", Temperature: " + Temperature + "°C, Humidity: " + Humidity + "%" + ", Pressure: " + Pressure + "hPa";
        }
    }
}
