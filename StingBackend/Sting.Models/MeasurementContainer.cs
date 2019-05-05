using System;
using System.Collections.Generic;
using System.Text;

namespace Sting.Models
{
    public class MeasurementContainer
    {
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Pressure { get; set; }

        public MeasurementContainer(double? temp = null, double? hum = null, double? press = null)
        {
            Temperature = temp;
            Humidity = hum;
            Pressure = press;
        }
    }
}
