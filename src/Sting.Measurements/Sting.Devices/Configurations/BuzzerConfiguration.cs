using Sting.Models.Configuration;

namespace Sting.Devices.Configurations
{
    public class BuzzerConfiguration : IDeviceConfiguration
    {
        public int PinNumber { get; set; }
        public int PwmChannel { get; set; }
    }
}
