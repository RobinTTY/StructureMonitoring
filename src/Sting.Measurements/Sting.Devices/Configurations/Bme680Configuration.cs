using System.Collections.Generic;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;

namespace Sting.Devices.Configurations
{
    public class Bme680Configuration : BmeBaseConfiguration
    {
        public bool HeaterIsEnabled { get; set; }
        public bool GasConversionIsEnabled { get; set; }
        public Bme680FilteringMode FilteringMode { get; set; }
        public List<Bme680HeaterConfiguration> HeaterProfiles { get; set; }
        public Bme680HeaterProfile ActiveProfile { get; set; }
    }

    public class Bme680HeaterConfiguration
    {
        public Bme680HeaterProfile HeaterProfile { get; set; }
        public ushort TargetTemperature { get; set; }
        public ushort Duration { get; set; }
    }
}
