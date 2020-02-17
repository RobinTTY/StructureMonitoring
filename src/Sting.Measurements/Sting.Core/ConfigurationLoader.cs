using System.Text.Json;
using Sting.Models.Configuration;

namespace Sting.Core
{
    public class ConfigurationLoader
    {
        private readonly JsonSerializerOptions _serializerOptions;

        public ConfigurationLoader()
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
