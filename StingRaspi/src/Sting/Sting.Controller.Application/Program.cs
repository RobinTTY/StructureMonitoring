using Sting.Storage;
using Sting.Controller.Application;

namespace Sting.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        private readonly Database _stingDatabase = new Database();

        // TODO: change to Net Core equivalent
        // private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();
        
        private static void Main(string[] args)
        {
            ContainerManager.InitializeApplication();
        }

        private void Configure()
        {
            // _stingDatabase.InitConnection();
            // _stingDatabase.SaveDocumentToCollection(bmpMeasurement.ToBsonDocument(), "TelemetryData");
        }
    }
}