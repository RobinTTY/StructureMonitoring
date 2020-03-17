using System.Collections.Generic;

namespace Sting.Models.Configurations
{
    public class SystemConfiguration
    {
        public ConfigInfo Info { get; set; }
        public ConfigDatabase Database { get; set; }
        public List<ConfigDevice> Devices { get; set; }

        
    }

    #region ConfigurationClasses
    public struct ConfigInfo
    {
        public string Version { get; set; }
    }

    public struct ConfigDatabase
    {
        public string Type { get; set; }
        public ConfigDbAttributes Attributes { get; set; }
    }

    public struct ConfigDbAttributes
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }

    public struct ConfigDevice
    {
        public string Name { get; set; }
        public IDeviceConfiguration Configuration { get; set; }
    }

    public interface IDeviceConfiguration {}

    #endregion
}
