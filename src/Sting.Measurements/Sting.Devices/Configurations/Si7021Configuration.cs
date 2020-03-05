using Iot.Device.Si7021;
using Sting.Models.Configurations;

namespace Sting.Devices.Configurations
{
    public class Si7021Configuration : I2CDeviceConfiguration, IDeviceConfiguration
    {
        public bool HeaterIsEnabled { get; set; }
        public Resolution Resolution { get; set; }
    }
}
