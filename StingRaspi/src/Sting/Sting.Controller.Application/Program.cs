namespace Sting.Controller.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal static class Program
    {
        // TODO: change to Net Core equivalent
        // private readonly EasClientDeviceInformation _deviceInfo = new EasClientDeviceInformation();

        private static void Main(string[] args)
        {
            ApplicationManager.StartApplication();
        }

        private static void StartApplication()
        {
            // _stingDatabase.InitConnection();
            // _stingDatabase.SaveDocumentToCollection(bmpMeasurement.ToBsonDocument(), "TelemetryData");
        }
    }
}