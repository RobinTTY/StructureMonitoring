using Sting.Models.Configurations;

namespace Sting.Devices.Configurations
{
    public class LedConfiguration : IDeviceConfiguration
    {
        public int PinNumber { get; set; }
    }
}
