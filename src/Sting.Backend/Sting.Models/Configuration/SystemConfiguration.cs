using System.Collections.Generic;

namespace Sting.Models.Configuration
{
    public class SystemConfiguration
    {
        public ConfigInfo Info;
        public ConfigDatabase Database;
        public List<ConfigSensor> Sensors;

        #region ConfigurationClasses
        public struct ConfigInfo
        {
            public string Version { get; set; }
        }

        public struct ConfigDatabase
        {
            public string Type { get; set; }

            public struct Attributes
            {
                public string Name { get; set; }
                public string ConnectionString { get; set; }
            }
        }

        public struct ConfigSensor
        {
            public string Name { get; set; }
        }
        #endregion
    }
}
