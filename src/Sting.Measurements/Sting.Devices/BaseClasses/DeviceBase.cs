using System.Text.Json;
using Sting.Devices.Contracts;

namespace Sting.Devices.BaseClasses
{
    public abstract class DeviceBase : IDevice
    {
        public string DeviceName { get; set; }
        public abstract bool Configure(string jsonDeviceConfiguration);

        // TODO: exception handling for missformed json
        protected static T DeserializeDeviceConfig<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
    }
}
