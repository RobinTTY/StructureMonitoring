using System;
using System.Text.Json;
using Sting.Core.Contracts;
using Sting.Models.Configuration;

namespace Sting.Core
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private readonly IDynamicComponentManager _componentManager;
        private readonly JsonSerializerOptions _serializerOptions;

        public ConfigurationLoader(IDynamicComponentManager componentManager)
        {
            _componentManager = componentManager;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
        }

        public void LoadConfiguration(SystemConfiguration configuration)
        {
            ConfigureDatabase(configuration.Database);
        }

        private void ConfigureDatabase(SystemConfiguration.ConfigDatabase config)
        {
            switch (config.Type)
            {
                case "MongoDB":
                    _componentManager.SetDatabase(new MongoDbDatabase(config.Attributes.Name, config.Attributes.ConnectionString));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        //public void ReadConfig()
        //{
        //    var path = File.ReadAllText(Path.Combine(SpecialDirectories.Desktop, "config.json"));
        //    var config = JsonSerializer.Deserialize<SystemConfiguration>(path, _serializerOptions);
        //}
    }
}
