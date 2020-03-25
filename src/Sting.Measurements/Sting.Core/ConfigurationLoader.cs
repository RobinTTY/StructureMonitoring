using System;
using System.Collections.Generic;
using System.Linq;
using Sting.Core.Communication;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using Sting.Devices.Sensors;

namespace Sting.Core
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private readonly IDynamicComponentManager _componentManager;
        private readonly ILogger _logger;
        private readonly Dictionary<string, Type> _deviceTypeNameMapping;

        public ConfigurationLoader(IDynamicComponentManager componentManager, ILogger logger)
        {
            _deviceTypeNameMapping = GetDeviceTypeNameMapping();
            _logger = logger;
            _componentManager = componentManager;
        }

        public void LoadConfiguration(SystemConfiguration configuration)
        {
            ConfigureDatabase(configuration.DatabaseConfig);
            ConfigureDevices(configuration.DeviceConfig.ToList());
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

        private void ConfigureDatabase(DatabaseConfig databaseConfig)
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

        private void ConfigureDevices(List<DeviceConfig> deviceConfigurations)
        {
            deviceConfigurations.ForEach(device =>
            {
                // TODO: configure device settings trough interface method of IDevice and pass SystemConfiguration child object that contains the needed information
                var deviceType = _deviceTypeNameMapping.FirstOrDefault(kvp => kvp.Key == device.Name).Value;
                var deviceObject = (IDevice)Activator.CreateInstance(deviceType);

                if (deviceObject != null)
                {
                    deviceObject.DeviceName = device.Name;
                    deviceObject.Configure(device.Configuration);
                    _componentManager.AddDevice(deviceObject);
                }
                else
                    _logger.Log($"Could not create object of type {device.Name}.");
            });
        }
    }
}
