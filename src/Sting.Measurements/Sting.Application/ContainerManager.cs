using Autofac;
using Sting.Application.Contracts;
using Sting.Core;
using Sting.Core.Contracts;
using Sting.Core.Logger;

namespace Sting.Application
{
    public static class ContainerManager
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer RegisterModules()
        {
            Builder.RegisterType<ApplicationManager>()
                .As<IApplicationManager>()
                .SingleInstance();

            Builder.RegisterType<SensorManager>()
                .As<ISensorManager>()
                .As<IService>()
                .SingleInstance();

            Builder.RegisterType<ConfigurationLoader>()
                .As<IConfigurationLoader>()
                .SingleInstance();

            Builder.RegisterType<DynamicComponentManager>()
                .As<IDynamicComponentManager>()
                .SingleInstance();

            Builder.RegisterType<DebugLineLogger>()
                .As<ILogger>()
                .SingleInstance();

            return Builder.Build();
        }
    }
}
