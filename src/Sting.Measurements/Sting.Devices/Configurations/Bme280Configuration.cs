using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;

namespace Sting.Devices.Configurations
{
    public class Bme280Configuration : BmeBaseConfiguration
    {
        public Bmx280FilteringMode FilterMode { get; set; }
        public StandbyTime StandbyTime { get; set; }
    }
}
