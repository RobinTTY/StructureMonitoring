using Iot.Device.Si7021;

namespace Sting.Devices.Configurations
{
    public class Si7021Configuration : I2CDeviceConfiguration
    {
        public bool HeaterIsEnabled { get; set; }
        public Resolution Resolution { get; set; }    }
}
