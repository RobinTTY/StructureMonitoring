using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sting.Units
{
    internal class Bmp180Data
    {
        public double Temperature { get; }
        public double Pressure { get; }
        public double Altitude { get; }

        public Bmp180Data(double temperature, double pressure, double altitude)
        {
            Temperature = temperature;
            Pressure = pressure;
            Altitude = altitude;
        }
    }
}
