using System;
using Sting.Controller.Contracts;
using Sting.Storage.Contracts;

namespace Sting.Controller.Application
{
    public class ApplicationManager
    {
        private ISensorManager _sensorManager;
        private IDatabase _database;

        private static bool IsInitialized;

        public ApplicationManager(ISensorManager sensorManager)
        {
            _sensorManager = sensorManager;
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
