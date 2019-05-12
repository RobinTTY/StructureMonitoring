using System;
using System.Collections.Generic;
using Sting.Core.Contracts;

namespace Sting.Application
{
    public class ApplicationManager
    {
        private IEnumerable<IService> _services;

        private static bool IsInitialized;

        public ApplicationManager(IEnumerable<IService> services)
        {
            _services = services;
        }

        public static void InitializeApplication()
        {
            ContainerManager.RegisterModules();

            IsInitialized = true;
        }

        public static void StartApplication()
        {
            if(!IsInitialized)
                throw new Exception("Application was not initialized correctly.");

            
        }

        public static void StopApplication()
        {

        }
    }
}
