using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Sting.Application.Contracts;
using Sting.Core.Contracts;
using Sting.Models.Configuration;

namespace Sting.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IEnumerable<IService> _services;
        private readonly IConfigurationLoader _configurationLoader;
        private readonly ManualResetEvent _waitEvent;

        public ApplicationManager(IEnumerable<IService> services, IConfigurationLoader configurationLoader)
        {
            _services = services;
            _configurationLoader = configurationLoader;
            _waitEvent = new ManualResetEvent(false);
        }

        public void StartApplication()
        {
            var path = File.ReadAllText(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "config.json"));
            var config = JsonSerializer.Deserialize<SystemConfiguration>(path);

            _configurationLoader.LoadConfiguration(config);
            _services.ToList().ForEach(service => service.Start());
            _waitEvent.WaitOne();
        }

        public void StopApplication()
        {
            _services.ToList().ForEach(service => service.Stop());
            _waitEvent.Set();
        }
    }
}
