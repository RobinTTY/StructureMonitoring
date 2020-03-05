using Sting.Devices.Contracts;
using Sting.Models.Configurations;

namespace Sting.Devices.BaseClasses
{
    public abstract class DeviceBase : IDevice
    {
        public string DeviceName { get; set; }
        public abstract bool Configure(IDeviceConfiguration deviceConfiguration);
    }
}
