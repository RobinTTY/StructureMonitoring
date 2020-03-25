using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sting.Application.Contracts;
using Sting.Core.Contracts;

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
            // TODO: setup grpc test application
            //_configurationLoader.LoadConfiguration(config);
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
