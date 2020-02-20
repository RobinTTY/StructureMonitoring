using System.Collections.Generic;

namespace Sting.Models.Configuration
{
    public class SystemConfiguration
    {
        public ConfigInfo Info { get; set; }
        public ConfigDatabase Database { get; set; }
        public List<ConfigDevices> Devices { get; set; }

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

        public struct ConfigDevices
        {
            public string Name { get; set; }
        }
        #endregion
    }
}
