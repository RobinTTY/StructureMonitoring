using Iot.Device.Bmp180;

namespace Sting.Devices.Configurations
{
    public class Bmp180Configuration : I2CDeviceConfiguration
    {
        public Sampling Sampling { get; set; }
    }
}
