using System.Collections.Generic;
using Autofac;
using Sting.Controller.Contracts;
using Sting.Devices.Contracts;

namespace Sting.Controller.Application
{
    public static class ContainerManager
    {
        public static void RegisterModules()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => new SensorManager(context.Resolve<IEnumerable<ISensorController>>()))
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();            
        }
    }
}
