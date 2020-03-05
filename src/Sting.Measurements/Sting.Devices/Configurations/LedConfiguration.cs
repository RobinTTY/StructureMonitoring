using Sting.Models.Configuration;

namespace Sting.Devices.Configurations
{
    public class LedConfiguration : IDeviceConfiguration
    {
        public int PinNumber { get; set; }
    }
}
