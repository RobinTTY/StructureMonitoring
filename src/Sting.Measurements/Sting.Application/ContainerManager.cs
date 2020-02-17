using Autofac;
using Sting.Application.Contracts;
using Sting.Core;
using Sting.Core.Contracts;
using Sting.Devices.Contracts;
using Sting.Devices.Sensors;
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
            Builder.RegisterType<ApplicationManager>()
                .As<IApplicationManager>()
                .SingleInstance();

            // TODO: probably handle as service
            Builder.Register(context => new MongoDbDatabase("Sting", "mongodb+srv://Robin:aBaJvCAXjJTvxDjcQtoh@stingmeasurements-2u2yu.mongodb.net/test?retryWrites=true&w=majority"))
                .As<IDatabase>()
                .SingleInstance();

            Builder.RegisterType<SensorManager>()
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

            Builder.RegisterType<Bme280Controller>()
                .As<Bme280Controller>()
                .As<ISensorController>()
                .SingleInstance();

            Builder.RegisterType<Bme680Controller>()
                .As<Bme680Controller>()
                .As<ISensorController>()
                .SingleInstance();

            return Builder.Build();
        }
    }
}
