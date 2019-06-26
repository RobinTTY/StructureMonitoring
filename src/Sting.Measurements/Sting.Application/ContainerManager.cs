using System.Collections.Generic;
using Autofac;
using Iot.Device.DHTxx;
using Sting.Application.Contracts;
using Sting.Core;
using Sting.Core.Contracts;
using Sting.Devices;
using Sting.Devices.Contracts;
using Sting.Persistence;
using Sting.Persistence.Contracts;

namespace Sting.Application
{
    public static class ContainerManager
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer RegisterModules()
        {
            Builder.Register(context => new ApplicationManager(context.Resolve<IEnumerable<IService>>()))
                .As<IApplicationManager>()
                .SingleInstance();

            // TODO: probably handle as service
            Builder.Register(context => new Database())
                .As<IDatabase>()
                .SingleInstance();

            Builder.Register(context => new SensorManager(context.Resolve<IEnumerable<ISensorController>>(), context.Resolve<IDatabase>()))
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();

            Builder.Register<IBmp180Controller>(context => new Bmp180Controller())
                .As<IBmp180Controller>()
                .As<ISensorController>()
                .SingleInstance();

            // TODO: configure through Configuration class, remove reference to Iot.Device.Binding
            //Builder.Register<IDhtController>(context => new DhtController(4, DhtType.Dht11))
            //    .As<IDhtController>()
            //    .As<ISensorController>()
            //    .SingleInstance();

            Builder.Register<IBme680Controller>(context => new Bme680Controller())
                .As<IBme680Controller>()
                .As<ISensorController>()
                .SingleInstance();

            return Builder.Build();
        }
    }
}
