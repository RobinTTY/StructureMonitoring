namespace Sting.Units

{
    public class Bmp180Data
    {
        public double Temperature { get; set;  }
        public double Pressure { get; set; }
        public double Altitude { get; set; }

        public Bmp180Data(double temperature, double pressure, double altitude)
        {
            Temperature = temperature;
            Pressure = pressure;
            Altitude = altitude;
        }
    }
}
