using System.Collections.Generic;
using System.Linq;
using Sting.Application.Contracts;
using Sting.Core.Contracts;

namespace Sting.Application
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IEnumerable<IService> _services;

        public ApplicationManager(IEnumerable<IService> services)
        {
            _services = services;
        }

        public void StartApplication()
        {
            _services.ToList().ForEach(service => service.Start());
        }

        public void StopApplication()
        {
            _services.ToList().ForEach(service => service.Stop());
        }
    }
}
