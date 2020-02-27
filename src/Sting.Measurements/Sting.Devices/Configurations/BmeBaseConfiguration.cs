using Iot.Device.Bmxx80;

namespace Sting.Devices.Configurations
{
    public class BmeBaseConfiguration : I2CDeviceConfiguration
    {
        public Sampling TemperatureSampling { get; set; }
        public Sampling PressureSampling { get; set; }
        public Sampling HumiditySampling { get; set; }
    }
}
