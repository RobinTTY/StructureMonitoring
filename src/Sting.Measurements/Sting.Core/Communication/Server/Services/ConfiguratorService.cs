using System.Threading.Tasks;
using Grpc.Core;
using Sting.Core.Contracts;

namespace Sting.Core.Communication.Server.Services
{
    public class ConfiguratorService : Configurator.ConfiguratorBase
    {
        private readonly IConfigurationLoader _configurationLoader;

        public ConfiguratorService(IConfigurationLoader configurationLoader)
        {
            _configurationLoader = configurationLoader;
        }

        public override Task<ConfigurationReply> ConfigureSystem(SystemConfiguration request, ServerCallContext context)
        {
            // TODO: handle failure to apply configuration
            _configurationLoader.LoadConfiguration(request);
            return Task.FromResult(new ConfigurationReply
            {
                Status = ConfigurationStatus.Applied,
                StatusMessage = "Configuration succeeded."
            });
        }
    }
}
