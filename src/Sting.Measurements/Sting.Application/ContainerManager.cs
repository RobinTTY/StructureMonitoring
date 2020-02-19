﻿using System.IO;
using Autofac;
using Microsoft.VisualBasic.FileIO;
using Sting.Application.Contracts;
using Sting.Core;
using Sting.Core.Contracts;

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

            // TODO: change pw
            Builder.Register(context => new MongoDbDatabase("Sting", File.ReadAllText(Path.Combine(SpecialDirectories.Desktop, "secret.txt"))))
                .As<IDatabase>()
                .SingleInstance();

            return Builder.Build();
        }
    }
}
