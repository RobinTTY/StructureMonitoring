using System;
using Sting.Models.Configuration;

namespace Sting.Devices.Configurations
{
    public class DhtConfiguration : IDeviceConfiguration
    {
        public Type DhtType { get; set; }
        public int PinNumber { get; set; }
    }
}
