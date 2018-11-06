using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Sensors.Dht;
using BMP;

namespace Sting.Measurements
{
    /// <summary>
    /// Represents a collection of telemetry data that can be collected
    /// trough different sensors.
    /// </summary>
    class TelemetryData
    {
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double Altitude { get; set; }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        public TelemetryData()
        {
            Timestamp = DateTime.Now;
            Temperature = double.NaN;
            Humidity = double.NaN;
            Pressure = double.NaN;
            Altitude = double.NaN;
        }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="data">A BMP180Data object</param>
        public TelemetryData(BMP180Data data)
        {
            Timestamp = DateTime.Now;
            Temperature = data.Temperature;
            Humidity = double.NaN;
            Pressure = data.Pressure;
            Altitude = data.Altitude;
        }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="data">A DhtReading object</param>
        public TelemetryData(DhtReading data)
        {
            Timestamp = DateTime.Now;
            Temperature = data.Temperature;
            Humidity = data.Humidity;
            Pressure = double.NaN;
            Altitude = double.NaN;
        }

        public void Complement(TelemetryData data)
        {
            PropertyInfo[] properties = typeof(TelemetryData).GetProperties();
            foreach (var property in properties)
            {
                if (!(property.GetValue(this) is double)) continue;
                if(double.IsNaN((double) property.GetValue(this)))
                    property.SetValue(this, property.GetValue(data));
            }
        }

        /// <summary>
        /// Converts the object to a Json string.
        /// </summary>
        /// <returns>Returns the serialized Json string.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            return "Time: " + Timestamp + ", Temperature: " + Temperature + "°C, Humidity: " + Humidity + "%," + ", Pressure: " + Pressure + "hPa," + " Altitude: " + Altitude + "m";
        }
    }
}
