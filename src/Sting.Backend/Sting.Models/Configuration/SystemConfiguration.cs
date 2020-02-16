using System.Collections.Generic;

namespace Sting.Models.Configuration
{
    public class SystemConfiguration
    {
        public Info Info;
        public Database Database;
        public List<Sensor> Sensors;
    }

    #region ConfigurationClasses
    public struct Info
    {
        public string Version { get; set; }
    }

    public struct Database
    {
        public string Type { get; set; }

        public struct Attributes
        {
            public string Name { get; set; }
            public string ConnectionString { get; set; }
        }
    }

    public struct Sensor
    {
        public string Name { get; set; }
    }
    #endregion
}
