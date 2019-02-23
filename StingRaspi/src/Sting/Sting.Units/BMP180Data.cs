namespace Sting.Units

{
    public class Bmp180Data
    {
        public double Temperature { get; }
        public double Pressure { get; }

        public Bmp180Data(double temperature, double pressure)
        {
            Temperature = temperature;
            Pressure = pressure;
        }
    }
}
