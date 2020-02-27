using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Sting.Models.Configuration;

namespace Sting.Devices.Configurations
{
    public class Bme280Configuration : BmeBaseConfiguration, IDeviceConfiguration
    {
        public Bmx280FilteringMode FilterMode { get; set; }
        public StandbyTime StandbyTime { get; set; }
    }
}
