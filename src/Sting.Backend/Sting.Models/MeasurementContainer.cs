namespace Sting.Models
{
    // TODO: change measurement container to be a dictionary so it doesn't have to be changed, look at how DB needs to support this
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
