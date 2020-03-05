using System;
using System.Collections.Generic;
using System.Linq;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using Sting.Devices.Sensors;
using Sting.Models.Configurations;

namespace Sting.Core
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private readonly IDynamicComponentManager _componentManager;
        private readonly Dictionary<string, Type> _deviceTypeNameMapping;

        public ConfigurationLoader(IDynamicComponentManager componentManager)
        {
            _deviceTypeNameMapping = GetDeviceTypeNameMapping();
            _componentManager = componentManager;
        }

        public void LoadConfiguration(SystemConfiguration configuration)
        {
            ConfigureDatabase(configuration.Database);
            ConfigureDevices(configuration.Devices);
        }

        private Dictionary<string, Type> GetDeviceTypeNameMapping()
        {
            // Get all Sensor and Actuator classes which implement IDevice
            return typeof(Bme280Controller).Assembly.GetTypes()
                .Where(type =>  type.IsClass 
                                && (type.Namespace == "Sting.Devices.Actuators" || type.Namespace == "Sting.Devices.Sensors") 
                                && type.GetInterfaces().Contains(typeof(IDevice)))
                .ToDictionary(type => type.Name, type => type);
        }

        private void ConfigureDatabase(ConfigDatabase databaseConfig)
        {
            switch (databaseConfig.Type)
            {
                case "MongoDB":
                    var db = new MongoDbDatabase(databaseConfig.Attributes.Name, databaseConfig.Attributes.ConnectionString);
                    _componentManager.SetDatabase(db);
                    db.Start();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void ConfigureDevices(List<ConfigDevice> deviceConfigurations)
        {
            deviceConfigurations.ForEach(device =>
            {
                // TODO: configure device settings trough interface method of IDevice and pass SystemConfiguration child object that contains the needed information
                var deviceType = _deviceTypeNameMapping.FirstOrDefault(kvp => kvp.Key == device.Name).Value;
                var deviceObject = (IDevice)Activator.CreateInstance(deviceType);

                deviceObject.DeviceName = device.Name;
                deviceObject.Configure(device.Configuration);
                _componentManager.AddDevice(deviceObject);
            });
        }
    }
}
