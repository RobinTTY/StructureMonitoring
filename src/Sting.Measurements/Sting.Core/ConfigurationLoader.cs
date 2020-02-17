using System.Text.Json;
using Sting.Core.Contracts;
using Sting.Models.Configuration;

namespace Sting.Core
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private readonly JsonSerializerOptions _serializerOptions;

        public ConfigurationLoader(ISensorManager sensorManager)
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
        }

        public void LoadConfiguration(SystemConfiguration configuration)
        {

        }

        //public void ReadConfig()
        //{
        //    var path = File.ReadAllText(Path.Combine(SpecialDirectories.Desktop, "config.json"));
        //    var config = JsonSerializer.Deserialize<SystemConfiguration>(path, _serializerOptions);
        //}
    }
}
