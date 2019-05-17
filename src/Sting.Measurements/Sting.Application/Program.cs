using Autofac;
using Sting.Application.Contracts;

namespace Sting.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        // TODO: change to Net Core equivalent
        // private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();

        private static void Main(string[] args)
        {
            var container = ContainerManager.RegisterModules();

            using var scope = container.BeginLifetimeScope();
                var applicationManager = scope.Resolve<IApplicationManager>();

            applicationManager.StartApplication();
        }
    }
}