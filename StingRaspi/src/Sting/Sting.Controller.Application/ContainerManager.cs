using System.Collections.Generic;
using Autofac;
using Sting.Devices;

namespace Sting.Controller.Application
{
    public static class ContainerManager
    {
        public static void InitializeApplication()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => new SensorManager(context.Resolve<IEnumerable<ISensorController>>()))
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();            
        }
    }
}
