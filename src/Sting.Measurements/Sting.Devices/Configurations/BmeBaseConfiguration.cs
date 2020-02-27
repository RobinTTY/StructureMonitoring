using Iot.Device.Bmxx80;

namespace Sting.Devices.Configurations
{
    public class BmeBaseConfiguration
    {
        // TODO: probably put in I2C base class
        public byte I2CAddress { get; set; }
        public Sampling TemperatureSampling { get; set; }
        public Sampling PressureSampling { get; set; }
        public Sampling HumiditySampling { get; set; }
    }
}
