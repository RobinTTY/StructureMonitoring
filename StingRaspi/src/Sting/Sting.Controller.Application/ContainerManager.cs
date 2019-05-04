using System.Collections.Generic;
using Autofac;
using Sting.Controller.Contracts;
using Sting.Devices.Contracts;

namespace Sting.Controller.Application
{
    class ContainerManager
    {
        public static void InitializeApplication()
        {
            var builder = new ContainerBuilder();

            builder.Register(context => new SensorManager(context.Resolve<IEnumerable<ISensorDevice>>()))
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();
        }
    }
}
