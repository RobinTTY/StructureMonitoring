using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Sting.Units
{
    /// <summary>
    /// Represents a collection of telemetry data that can be collected
    /// trough different sensors.
    /// </summary>
    public class TelemetryData
    {
        public long UnixTimeStampMilliseconds { get; }
        public double Temperature { get; }
        public double Humidity { get; }
        public double Pressure { get; }

        /// <summary>
        /// Represents a collection of telemetry data that can be collected
        /// trough different sensors.
        /// </summary>
        /// <param name="temperature">temperature in °C</param>
        /// <param name="humidity">humidity in percent</param>
        /// <param name="pressure">absolute pressure</param>
        public TelemetryData(double temperature = double.NaN, double humidity = double.NaN, double pressure = double.NaN)
        {          
            UnixTimeStampMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Temperature = temperature;
            Humidity = humidity;
            Pressure = pressure;
        }

        /// <summary>
        /// Adds to the existing telemetry data, data that was not measured yet.
        /// </summary>
        /// <param name="data">A TelemetryData object.</param>
        public void Complement(TelemetryData data)
        {
            if (data == null) return;
            PropertyInfo[] properties = typeof(TelemetryData).GetProperties();
            foreach (var property in properties)
            {
                if (!(property.GetValue(this) is double)) continue;
                if(double.IsNaN((double) property.GetValue(this)))
                    property.SetValue(this, property.GetValue(data));
            }
        }

        /// <summary>
        /// Overwrites the existing telemetry data, with data that is present
        /// in the passed in element.
        /// </summary>
        /// <param name="data">A TelemetryData object.</param>
        public void Overwrite(TelemetryData data)
        {
            if (data == null) return;
            PropertyInfo[] properties = typeof(TelemetryData).GetProperties();
            foreach (var property in properties)
            {
                if (!(property.GetValue(this) is double))
                {
                    property.SetValue(this, property.GetValue(data));
                    continue;
                }

                if (double.IsNaN((double) property.GetValue(data)))
                    continue;

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
            return "Temperature: " + Temperature + "°C, Humidity: " + Humidity + "%, Pressure: " + Pressure + "hPa, Altitude: ";
        }
    }
}
