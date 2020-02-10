using System.Collections.Generic;
using Autofac;
using Sting.Application.Contracts;
using Sting.Core;
using Sting.Core.Contracts;
using Sting.Devices;
using Sting.Devices.Contracts;
using Sting.Persistence;
using Sting.Persistence.Contracts;

namespace Sting.Application
{
    // TODO: some sort of configuration procedure instead of constructors?!
    public static class ContainerManager
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer RegisterModules()
        {
            Builder.Register(context => new ApplicationManager(context.Resolve<IEnumerable<IService>>()))
                .As<IApplicationManager>()
                .SingleInstance();

            // TODO: probably handle as service
            Builder.Register(context => new MongoDBDatabase("Sting", ""))
                .As<IDatabase>()
                .SingleInstance();

            Builder.Register(context => new SensorManager(context.Resolve<IEnumerable<ISensorController>>(), context.Resolve<IDatabase>()))
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();

            // TODO: configure through Configuration class, remove reference to Iot.Device.Binding
            //Builder.Register(context => new DhtController(4))
            //    .As<DhtController>()
            //    .As<ISensorController>()
            //    .SingleInstance();

            //Builder.Register(context => new Bmp180Controller())
            //    .As<Bmp180Controller>()
            //    .As<ISensorController>()
            //    .SingleInstance();

            Builder.Register(context => new Bme280Controller())
                .As<Bme280Controller>()
                .As<ISensorController>()
                .SingleInstance();

            Builder.Register(context => new Bme680Controller())
                .As<Bme680Controller>()
                .As<ISensorController>()
                .SingleInstance();

            return Builder.Build();
        }
    }
}
